using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.DeviceName.PLC;
using NXP_Stocker_BlazorProject.TaskPackage.MainTaskPackage;
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

        public MainTaskPack<EPLC> mainTaskPack;

        public MainTask mainTask;

        void initMainTask()
        {
            mainTaskPack = new MainTaskPack<EPLC>(EPLC.Pier1, EPLC.Pier2, EPLC.Robot, plcLibrary, 
                                                  mainDataService, observerService);

            mainTask = new MainTask(mainTaskPack, pier1MissionAsignTask, pier2MissionAsignTask,
                                    pier1Task, pier2Task, robotTask);

            mainTask.Set(ES1.Init, EMain.None, 0);
        }


        private CancellationTokenSource _cts;
        private Task _loopTask;

        void initThread()
        {
            if (_loopTask == null || _loopTask.IsCompleted)
            {
                _cts = new CancellationTokenSource();
                _loopTask = RunAsync(_cts.Token);
            }
        }

        async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await mainTask.Run(); // 你的主邏輯

                    await Task.Delay(1, token); // 可取消的 delay
                }
                catch (TaskCanceledException)
                {
                    // 正常中止
                }
                catch (Exception ex)
                {
                    // 記錄錯誤
                    Console.WriteLine($"錯誤：{ex.Message}");
                }
            }
        }

        public void StopThread()
        {
            _cts?.Cancel();
        }

    }
}
