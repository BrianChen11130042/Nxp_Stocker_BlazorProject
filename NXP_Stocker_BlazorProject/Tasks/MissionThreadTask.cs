using CommonLibraryB_NXP.Base.FiniteStateMachine;
using DevExpress.Utils.Filtering.Internal;
using NXP_Stocker_BlazorProject.TaskPackage.ThreadTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class MissionThreadTask
    {
        readonly PierTask pier1Task;
        readonly PierTask pier2Task;

        readonly RobotTask robotTask;

        public MissionThreadTask(PierTask pier1Task, PierTask pier2Task, RobotTask robotTask)
        {
            this.pier1Task = pier1Task;
            this.pier2Task = pier2Task;

            this.robotTask = robotTask;

            interval = 1;
        }
    }

    public enum EMissionThread
    {
        None,
        Mission
    }

    public partial class MissionThreadTask : FSMBase<EMissionThread, int>
    {
        public async override Task Init()
        {
            pier1Task.Set(ES1.Action, EPierAction.CheckMission, 0);
            pier2Task.Set(ES1.Action, EPierAction.CheckMission, 0);

            robotTask.Set(ES1.Action, ERobotAction.CheckReady, 0);

            Set(ES1.Action, EMissionThread.Mission, 0);
        }

        public async override Task Action()
        {
            switch(S2)
            {
                case EMissionThread.None:
                    Set(ES1.Finish, EMissionThread.None, 0);
                    break;

                case EMissionThread.Mission:
                    switch(S3)
                    {
                        case 0:
                            await pier1Task.Run();

                            if (pier1Task.key == EHandshakeKey.Finish)
                            {
                                if (pier1Task.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMissionThread.None, 0);
                                }
                                else
                                {
                                    Set(10);
                                }
                            }
                            else
                            {
                                Set(10);
                            }
                            break;

                        case 10:
                            await pier2Task.Run();

                            if (pier2Task.key == EHandshakeKey.Finish)
                            {
                                if (pier2Task.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMissionThread.None, 0);
                                }
                                else
                                {
                                    Set(20);
                                }
                            }
                            else
                            {
                                Set(20);
                            }
                            break;

                        case 20:
                            await robotTask.Run();

                            if (robotTask.key == EHandshakeKey.Finish)
                            {
                                if (robotTask.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMissionThread.None, 0);
                                }
                                else
                                {
                                    Set(0);
                                }
                            }
                            else
                            {
                                Set(0);
                            }
                            break;
                    }
                    break; 
            }
        }

        public async override Task Error()
        {
            isError = true;
            key = EHandshakeKey.Finish;
            Set(ES1.Idle, EMissionThread.None, 0);
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
