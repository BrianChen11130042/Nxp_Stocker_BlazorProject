using CommonLibraryB.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{

    public partial class PierTask : IPierTaskPack
    {

        readonly IPierTaskPack pack;

        public PierTask(IPierTaskPack pack)
        {
            this.pack = pack;
            interval = 10;
        }

        public Task<bool> GetPlcInputLargeBoardStatus()
        {
            return pack.GetPlcInputLargeBoardStatus();
        }

        public Task<bool> GetPlcPierName()
        {
            return pack.GetPlcPierName();
        }

        public Task<bool> GetTableNewMission()
        {
            return pack.GetTableNewMission();
        }

        public bool IsInputLargeBoard()
        {
            return pack.IsInputLargeBoard();
        }

        public bool IsInputLargeBoardFinish()
        {
            return pack.IsInputLargeBoardFinish();
        }

        public bool IsInputSmallBoard()
        {
            return pack.IsInputSmallBoard();
        }

        public bool IsOutputLargeBoard()
        {
            return pack.IsOutputLargeBoard();
        }

        public bool IsOutputSmallBoard()
        {
            return pack.IsOutputSmallBoard();
        }

        public Task<bool> SetLogMissionFinish()
        {
            return pack.SetLogMissionFinish();
        }

        public Task<bool> SetLogMissionStart()
        {
            return pack.SetLogMissionStart();
        }

        public Task<bool> SetPlcFinshInputLargeBoard()
        {
            return pack.SetPlcFinshInputLargeBoard();
        }

        public Task<bool> SetPlcStartInputLargeBoard()
        {
            return pack.SetPlcStartInputLargeBoard();
        }

        public Task<bool> SetTableMissionFinsih()
        {
            return pack.SetTableMissionFinsih();
        }

        public Task<bool> SetTableMissionStart()
        {
            return pack.SetTableMissionStart();
        }

        public Task UpdateUIPierLog()
        {
            return pack.UpdateUIPierLog();
        }

        public Task UpdateUIPierMission()
        {
            return pack.UpdateUIPierMission();
        }
    }

    public enum EPierAction
    {
        None,
        CheckMission,
        InputLargeBoard,
        InputSmallBoard,
        OutputLargeBoard,
        OutputSmallBoard,
    }

    public partial class PierTask : FSMBase<EPierAction, int>
    {
        public async override Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case EPierAction.None:
                    Set(ES1.Finish, EPierAction.None, 0);
                    break;

                case EPierAction.CheckMission:
                    switch(S3)
                    {
                        case 0:
                            if(await GetPlcPierName())
                            {
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await GetTableNewMission())
                            {
                                await UpdateUIPierMission();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(IsInputLargeBoard())
                            {
                                Set(EPierAction.InputLargeBoard, 0);
                            }
                            else if(IsInputSmallBoard())
                            {
                                Set(EPierAction.InputSmallBoard, 0);
                            }
                            else if(IsOutputLargeBoard())
                            {
                                Set(EPierAction.OutputLargeBoard, 0);
                            }
                            else if(IsOutputSmallBoard())
                            {
                                Set(EPierAction.OutputSmallBoard, 0);
                            }
                            else
                            {
                                Set(EPierAction.CheckMission, 0);
                            }
                            break;
                    }
                    break;

                case EPierAction.InputLargeBoard:
                    switch (S3)
                    {
                        case 0:
                            if (await SetPlcStartInputLargeBoard())
                            {
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIPierMission();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await SetLogMissionStart())
                            {
                                await UpdateUIPierLog();
                                Set(30);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 30:
                            if(await GetPlcInputLargeBoardStatus())
                            {
                                await UpdateUIPierMission();
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 40:
                            if(IsInputLargeBoardFinish())
                            {
                                Set(50);
                            }
                            else
                            {
                                Set(30);
                            }
                            break;

                        case 50:
                            if(await SetPlcFinshInputLargeBoard())
                            {
                                Set(60);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 60:
                            if(await SetTableMissionFinsih())
                            {
                                await UpdateUIPierMission();
                                Set(70);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 70:
                            if(await SetLogMissionFinish())
                            {
                                await UpdateUIPierLog();
                                Set(EPierAction.CheckMission, 0);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
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
            Set(ES1.Idle, EPierAction.None, 0);
        }

        public async override Task Finish()
        {
            isError = false;
            key = EHandshakeKey.Finish;
            Set(ES1.Idle, EPierAction.None, 0);
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
