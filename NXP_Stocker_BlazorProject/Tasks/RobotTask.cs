using CommonLibraryB.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{

    public partial class RobotTask : IRobotTaskPack
    {
        readonly IRobotTaskPack pack;

        public RobotTask(IRobotTaskPack pack)
        {
            this.pack = pack;
            interval = 10;
        }

        public Task<bool> GetRobotStatus()
        {
            return pack.GetRobotStatus();
        }

        public Task<bool> GetTableNewMission()
        {
            return pack.GetTableNewMission();
        }

        public bool IsGetNewMission()
        {
            return pack.IsGetNewMission();
        }

        public bool IsRobotError()
        {
            return pack.IsRobotError();
        }

        public Task UpdateUIRobotMission()
        {
            return pack.UpdateUIRobotMission();
        }
    }

    public enum ERobotAction
    {
        None,
        CheckError,
        CheckMission,
        Start,
        Result,
        Success,
        Fail,
    }

    public partial class RobotTask : FSMBase<ERobotAction, int>
    {
        public async override Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case ERobotAction.None:
                    Set(ES1.Finish, ERobotAction.None, 0);
                    break;

                case ERobotAction.CheckError:
                    switch(S3)
                    {
                        case 0:
                            if (await GetRobotStatus())
                            {
                                if (IsRobotError())
                                {
                                    Set(ERobotAction.CheckError, 0);
                                }
                                else
                                {
                                    Set(ERobotAction.CheckMission, 0);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, ERobotAction.None, 0);
                            }
                            break;
                    }
                    break;

                case ERobotAction.CheckMission:
                    switch(S3)
                    {
                        case 0:
                            if (await GetTableNewMission())
                            {
                                await UpdateUIRobotMission();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, ERobotAction.None, 0);
                            }
                            break;

                        case 10:
                            if(IsGetNewMission())
                            {
                                Set(20);
                            }
                            else
                            {
                                Set(ERobotAction.CheckError, 0);
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
            Set(ES1.Idle, ERobotAction.None, 0);
        }

        public async override Task Finish()
        {
            isError = false;
            key = EHandshakeKey.Finish;
            Set(ES1.Idle, ERobotAction.None, 0);
        }

        public async override Task Idle()
        {
            //throw new NotImplementedException();
        }

        public async override Task Init()
        {
            //throw new NotImplementedException();
        }
    }
}
