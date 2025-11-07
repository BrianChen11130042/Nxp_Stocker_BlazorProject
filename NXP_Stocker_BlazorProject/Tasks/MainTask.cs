using CommonLibraryB.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.MainTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class MainTask : IMainTaskPack
    {
        readonly IMainTaskPack pack;

        readonly MissionAsignTask pier1AsignTask;
        readonly MissionAsignTask pier2AsignTask;

        readonly PierTask pier1Task;
        readonly PierTask pier2Task;

        readonly RobotTask robotTask;

        public MainTask(IMainTaskPack pack, MissionAsignTask pier1AsignTask, MissionAsignTask pier2AsignTask,
                        PierTask pier1Task, PierTask pier2Task, RobotTask robotTask)
        {
            this.pack = pack;

            this.pier1AsignTask = pier1AsignTask;
            this.pier2AsignTask = pier2AsignTask;

            this.pier1Task = pier1Task;
            this.pier2Task = pier2Task;

            this.robotTask = robotTask;

            interval = 1;
        }

        public Task<bool> CheckDbConnect()
        {
            return pack.CheckDbConnect();
        }

        public Task<bool> CheckPlcConnect()
        {
            return pack.CheckPlcConnect();
        }

        public Task<bool> SetLogInitFail()
        {
            return pack.SetLogInitFail();
        }

        public Task<bool> SetLogInitSuccess()
        {
            return pack.SetLogInitSuccess();
        }

        public Task UpdateUIInitFail()
        {
            return pack.UpdateUIInitFail();
        }

        public Task UpdateUIInitSuccess()
        {
            return pack.UpdateUIInitSuccess();
        }
    }

    public enum EMain
    {
        None,
        HeartBeat,
        MissionAsign,
        Mission,
    }

    public partial class MainTask : FSMBase<EMain, int>
    {
        public async override Task Init()
        {
            switch(S3)
            {
                case 0:
                    if (await CheckDbConnect())
                    {
                        Set(10);
                    }
                    else
                    {
                        Set(30);
                    }
                    break;

                case 10:
                    if(await CheckPlcConnect())
                    {
                        Set(20);
                    }
                    else
                    {
                        Set(30);
                    }
                    break;

                case 20:
                    if(await SetLogInitSuccess())
                    {
                        await UpdateUIInitSuccess();

                        pier1AsignTask.Set(ES1.Action, EMissionAssign.CheckMission, 0);
                        pier2AsignTask.Set(ES1.Action, EMissionAssign.CheckMission, 0);

                        pier1Task.Set(ES1.Action, EPierAction.CheckMission, 0);
                        pier2Task.Set(ES1.Action, EPierAction.CheckMission, 0);

                        robotTask.Set(ES1.Action, ERobotAction.CheckError, 0);

                        Set(ES1.Action, EMain.HeartBeat, 0);
                    }
                    else
                    {
                        Set(30);
                    }
                    break;

                case 30:
                    await SetLogInitFail();
                    await UpdateUIInitFail();

                    Set(ES1.None, EMain.None, 0);
                    break;
            }
        }

        public override Task Action()
        {
            throw new NotImplementedException();
        }

        public override Task Error()
        {
            throw new NotImplementedException();
        }

        public async override Task Finish()
        {
            //throw new NotImplementedException();
        }

        public async override Task Idle()
        {
            //throw new NotImplementedException();
        }

    }
}
