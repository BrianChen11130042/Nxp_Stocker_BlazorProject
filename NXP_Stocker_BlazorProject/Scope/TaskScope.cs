using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.DeviceName.PLC;
using NXP_Stocker_BlazorProject.TaskPackage.ThreadTaskPackage;
using NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage;
using NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage;
using NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage;
using NXP_Stocker_BlazorProject.Tasks;

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


        public MissionAsignThreadTask missionAsignThreadTask;
        public MissionThreadTask missionThreadTask;

        public ThreadTaskPack<EPLC> mainTaskPack;
        public MainThreadTask mainThreadTask;

        void initThreadTask()
        {


            mainTaskPack = new ThreadTaskPack<EPLC>(EPLC.Pier1, EPLC.Pier2, EPLC.Robot, plcLibrary, 
                                                    mainDataService, observerService);

            missionAsignThreadTask = new MissionAsignThreadTask(pier1MissionAsignTask, pier2MissionAsignTask);
            missionAsignThreadTask.Set(ES1.None, EMissionAsignThread.None, 0);

            missionThreadTask = new MissionThreadTask(pier1Task, pier2Task, robotTask);
            missionThreadTask.Set(ES1.None, EMissionThread.None, 0);

            mainThreadTask = new MainThreadTask(mainTaskPack, missionAsignThreadTask, missionThreadTask);
            mainThreadTask.Set(ES1.Init, EMainThread.None, 0);
        }

        private CancellationTokenSource _ctsMain;
        private CancellationTokenSource _ctsMissionAssign;
        private CancellationTokenSource _ctsMission;

        private Task _mainTask;
        private Task _missionAssignTask;
        private Task _missionTask;

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

                    await Task.Delay(600, token); // 不要 Thread.Sleep
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

                    await Task.Delay(10, token);
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"Assign 例外: {ex}");
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

                    await Task.Delay(70, token);
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"Mission 例外: {ex}");
                }
            }
        }

        public void StopThread()
        {
            _ctsMain?.Cancel();
            _ctsMissionAssign?.Cancel();
            _ctsMission?.Cancel();
        }

    }
}
