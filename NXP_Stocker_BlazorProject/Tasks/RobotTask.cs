using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{

    public partial class RobotTask : IRobotTaskPack
    {
        readonly IRobotTaskPack pack;

        public RobotTask(IRobotTaskPack pack)
        {
            this.pack = pack;
            interval = 1;
        }

        public Task<bool> GetPlcRobotStatus()
        {
            return pack.GetPlcRobotStatus();
        }

        public Task<bool> GetRobotIsReady()
        {
            return pack.GetRobotIsReady();
        }

        public Task<bool> GetTableNewMission()
        {
            return pack.GetTableNewMission();
        }

        public bool IsGetNewMission()
        {
            return pack.IsGetNewMission();
        }

        public bool IsRobotReady()
        {
            return pack.IsRobotReady();
        }

        public bool IsRobotFinish()
        {
            return pack.IsRobotFinish();
        }

        public Task<bool> SetLogMissionFinish()
        {
            return pack.SetLogMissionFinish();
        }

        public Task<bool> SetLogMissionStart()
        {
            return pack.SetLogMissionStart();
        }

        public Task<bool> SetPlcRobotFinish()
        {
            return pack.SetPlcRobotFinish();
        }

        public Task<bool> SetPlcRobotMission()
        {
            return pack.SetPlcRobotMission();
        }

        public Task<bool> SetPlcRobotStart()
        {
            return pack.SetPlcRobotStart();
        }

        public Task<bool> SetTableMissionFinish()
        {
            return pack.SetTableMissionFinish();
        }

        public Task<bool> SetTableMissionStart()
        {
            return pack.SetTableMissionStart();
        }

        public Task UpdateRobotMissionStatusToInQue()
        {
            return pack.UpdateRobotMissionStatusToInQue();
        }

        public Task UpdateUIRobotLog()
        {
            return pack.UpdateUIRobotLog();
        }

        public Task UpdateUIRobotMission()
        {
            return pack.UpdateUIRobotMission();
        }
    }

    public enum ERobotAction
    {
        None,
        CheckReady,
        CheckMission,
        Start,
        GetResult,
        Finish,
        Error,
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

                case ERobotAction.CheckReady:
                    switch(S3)
                    {
                        case 0:
                            if (await GetRobotIsReady())
                            {
                                if (IsRobotReady())
                                {
                                    Set(ERobotAction.CheckMission, 0);
                                }
                                else
                                {
                                    Set(ERobotAction.CheckReady, 0);
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

                                if (IsGetNewMission())
                                {
                                    Set(ERobotAction.Start, 0);
                                }
                                else
                                {
                                    Set(ERobotAction.CheckReady, 0);
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

                case ERobotAction.Start:
                    switch(S3)
                    {
                        case 0:
                            if(await SetPlcRobotMission())
                            {
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, ERobotAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await SetPlcRobotStart())
                            {
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, ERobotAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIRobotMission();
                                Set(30);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, ERobotAction.None, 0);
                            }
                            break;

                        case 30:
                            if(await SetLogMissionStart())
                            {
                                await UpdateUIRobotLog();
                                Set(ERobotAction.GetResult, 0);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, ERobotAction.None, 0);
                            }
                            break;
                    }
                    break;

                case ERobotAction.GetResult:
                    switch(S3)
                    {
                        case 0:
                            if(await GetPlcRobotStatus())
                            {
                                await UpdateRobotMissionStatusToInQue();
                                await UpdateUIRobotMission();

                                if (IsRobotFinish())
                                {
                                    Set(ERobotAction.Finish, 0);
                                }
                                else
                                {
                                    Set(0);
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

                case ERobotAction.Finish:
                    switch(S3)
                    {
                        case 0:
                            if(await SetPlcRobotFinish())
                            {
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, ERobotAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await SetTableMissionFinish())
                            {
                                await UpdateUIRobotMission();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, ERobotAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await SetLogMissionFinish())
                            {
                                await UpdateUIRobotLog();
                                Set(ERobotAction.CheckReady, 0);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, ERobotAction.None, 0);
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
