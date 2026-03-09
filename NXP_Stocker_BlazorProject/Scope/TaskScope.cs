using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.DeviceName.PLC;
using NXP_Stocker_BlazorProject.TaskPackage.ThreadTaskPackage;
using NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage;
using NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage;
using NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage;
using NXP_Stocker_BlazorProject.TaskPackage.PlcRegularTaskPackage;
using NXP_Stocker_BlazorProject.Tasks;
using NXP_Stocker_BlazorProject.DeviceName.UPS;
using NXP_Stocker_BlazorProject.TaskPackage.UpsRegularTaskPackage;
using NXP_Stocker_BlazorProject.TaskPackage.UpsRegularTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Scope
{
    public partial class MachineScope
    {
        public PierTaskPack<EPLC> pier1TaskPack;
        public PierTaskPack<EPLC> pier2TaskPack;

        public PierTask pier1Task;
        public PierTask pier2Task;

        void initPierTask()
        {
            pier1TaskPack = new PierTaskPack<EPLC>(EPLC.Pier1, plcLibrary, pier1DataService, observerService);
            pier2TaskPack = new PierTaskPack<EPLC>(EPLC.Pier2, plcLibrary, pier2DataService, observerService);

            pier1Task = new PierTask(pier1TaskPack);
            pier2Task = new PierTask(pier2TaskPack);

            pier1Task.Set(ES1.None, EPierAction.None, 0);
            pier2Task.Set(ES1.None, EPierAction.None, 0);
        }

        public RobotTaskPack<EPLC> robotTaskPack;

        public RobotTask robotTask;

        void initRobotTask()
        {
            robotTaskPack = new RobotTaskPack<EPLC>(EPLC.Robot, plcLibrary, robotDataService, observerService);

            robotTask = new RobotTask(robotTaskPack);

            robotTask.Set(ES1.None, ERobotAction.None, 0);
        }

        public MissionAsignTaskPack<EPLC> pier1MissionAsignTaskPack;
        public MissionAsignTaskPack<EPLC> pier2MissionAsignTaskPack;

        public MissionAsignTask pier1MissionAsignTask;
        public MissionAsignTask pier2MissionAsignTask;

        void initMissionAsignTask()
        {
            pier1MissionAsignTaskPack = new MissionAsignTaskPack<EPLC>(EPLC.Pier1, plcLibrary,
                                                                       pier1MissionAsignDataService, observerService);

            pier2MissionAsignTaskPack = new MissionAsignTaskPack<EPLC>(EPLC.Pier2, plcLibrary,
                                                                       pier2MissionAsignDataService, observerService);

            pier1MissionAsignTask = new MissionAsignTask(pier1MissionAsignTaskPack);
            pier2MissionAsignTask = new MissionAsignTask(pier2MissionAsignTaskPack);

            pier1MissionAsignTask.Set(ES1.None, EMissionAssign.None, 0);
            pier2MissionAsignTask.Set(ES1.None, EMissionAssign.None, 0);
        }

        public PlcRegularTaskPack<EPLC> plcRegularTaskPack;
        public UpsRegularTaskPack<EUPS> upsRegularTaskPack;

        public PlcRegularTask plcRegularTask;
        public UpsRegularTask upsRegularTask;

        void initRegularTask()
        {
            plcRegularTaskPack = new PlcRegularTaskPack<EPLC>(EPLC.Warehouse, EPLC.Heartbeat, plcLibrary,
                                                              plcRegularDataService, observerService);

            upsRegularTaskPack = new UpsRegularTaskPack<EUPS>(EUPS.UPS, upsLibrary,
                                                              upsRegularDataService, observerService);

            plcRegularTask = new PlcRegularTask(plcRegularTaskPack);
            plcRegularTask.Set(ES1.None, EPlcRegular.None, 0);

            upsRegularTask = new UpsRegularTask(upsRegularTaskPack);
            upsRegularTask.Set(ES1.None, EUpsRegular.None, 0);
        }


        public MissionAsignThreadTask missionAsignThreadTask;
        public MissionThreadTask missionThreadTask;
        public PlcRegularThreadTask plcRegularThreadTask;
        public UpsRegularThreadTask upsRegularThreadTask;

        public ThreadTaskPack<EPLC, EUPS> mainTaskPack;
        public MainThreadTask mainThreadTask;

        void initThreadTask()
        {

            mainTaskPack = new ThreadTaskPack<EPLC, EUPS>(EPLC.Pier1, EPLC.Pier2, EPLC.Robot, plcLibrary,
                                                          EUPS.UPS, upsLibrary,
                                                          mainDataService, observerService);

            missionAsignThreadTask = new MissionAsignThreadTask(pier1MissionAsignTask, pier2MissionAsignTask);
            missionAsignThreadTask.Set(ES1.None, EMissionAsignThread.None, 0);

            missionThreadTask = new MissionThreadTask(pier1Task, pier2Task, robotTask);
            missionThreadTask.Set(ES1.None, EMissionThread.None, 0);

            plcRegularThreadTask = new PlcRegularThreadTask(plcRegularTask);
            plcRegularThreadTask.Set(ES1.None, EPlcRegularThread.None, 0);

            upsRegularThreadTask = new UpsRegularThreadTask(upsRegularTask);
            upsRegularThreadTask.Set(ES1.None, EUpsRegularThread.None, 0);

            mainThreadTask = new MainThreadTask(mainTaskPack, missionAsignThreadTask, missionThreadTask,
                                                plcRegularThreadTask, upsRegularThreadTask);
            mainThreadTask.Set(ES1.Init, EMainThread.None, 0);
        }

        private CancellationTokenSource _ctsMain;
        private CancellationTokenSource _ctsMissionAssign;
        private CancellationTokenSource _ctsMission;
        private CancellationTokenSource _ctsPlcRegular;
        private CancellationTokenSource _ctsUpsRegular;

        private Task _mainTask;
        private Task _missionAssignTask;
        private Task _missionTask;
        private Task _plcRegularTask;
        private Task _upsRegularTask;

        public void initThread()
        {
            if (_mainTask == null || _mainTask.IsCompleted)
            {
                _ctsMain = new CancellationTokenSource();
                _mainTask = StartLongRunning(async () => await RunMainAsync(_ctsMain.Token));
            }

            if (_missionAssignTask == null || _missionAssignTask.IsCompleted)
            {
                _ctsMissionAssign = new CancellationTokenSource();
                _missionAssignTask = StartLongRunning(async () => await RunMissionAssignAsync(_ctsMissionAssign.Token));
            }

            if (_missionTask == null || _missionTask.IsCompleted)
            {
                _ctsMission = new CancellationTokenSource();
                _missionTask = StartLongRunning(async () => await RunMissionAsync(_ctsMission.Token));
            }

            if (_plcRegularTask == null || _plcRegularTask.IsCompleted)
            {
                _ctsPlcRegular = new CancellationTokenSource();
                _plcRegularTask = StartLongRunning(async () => await RunPlcRegularAsync(_ctsPlcRegular.Token));
            }

            if (_upsRegularTask == null || _upsRegularTask.IsCompleted)
            {
                _ctsUpsRegular = new CancellationTokenSource();
                _upsRegularTask = StartLongRunning(async () => await RunUpsRegularAsync(_ctsUpsRegular.Token));
            }
        }

        private Task StartLongRunning(Func<Task> func)
        {
            // 開啟「專屬 thread」而不是 ThreadPool
            return Task.Factory.StartNew(
                async () => await func(),
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
            ).Unwrap();
        }

        private async Task RunMainAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await mainThreadTask.Run();

                    await Task.Delay(300, token); // 不要 Thread.Sleep
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"Main 例外: {ex}");
                }
            }
        }

        private async Task RunMissionAssignAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await missionAsignThreadTask.Run();

                    await Task.Delay(5, token);
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"Mission Assign 例外: {ex}");
                }
            }
        }

        private async Task RunMissionAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await missionThreadTask.Run();

                    await Task.Delay(50, token);
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"Mission 例外: {ex}");
                }
            }
        }

        private async Task RunPlcRegularAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await plcRegularThreadTask.Run();

                    await Task.Delay(500, token);
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"Plc Regular 例外: {ex}");
                }
            }
        }

        private async Task RunUpsRegularAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await upsRegularThreadTask.Run();

                    await Task.Delay(2000, token);
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ups Regular 例外: {ex}");
                }
            }
        }

        public void StopThread()
        {
            _ctsMain?.Cancel();
            _ctsMissionAssign?.Cancel();
            _ctsMission?.Cancel();
            _ctsPlcRegular?.Cancel();
            _ctsUpsRegular?.Cancel();

            // 強制等待所有 Task 結束，最多等 2 秒，避免死結
            Task.WaitAll(new[] {_mainTask, _missionAssignTask, _missionTask, _plcRegularTask, _upsRegularTask}
                        .Where(t => t != null)
                        .ToArray(), TimeSpan.FromSeconds(60));

        }

    }
}
