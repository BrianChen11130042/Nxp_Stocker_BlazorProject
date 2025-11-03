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

        public Task<bool> GetPlcInputSmallBoardStatus()
        {
            return pack.GetPlcInputSmallBoardStatus();
        }

        public Task<bool> GetPlcOutputLargeBoardStatus()
        {
            return pack.GetPlcOutputLargeBoardStatus();
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

        public bool IsInputSmallBoardFinish()
        {
            return pack.IsInputSmallBoardFinish();
        }

        public bool IsOutputLargeBoard()
        {
            return pack.IsOutputLargeBoard();
        }

        public bool IsOutputLargeBoardFinish()
        {
            return pack.IsOutputLargeBoardFinish();
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

        public Task<bool> SetPlcFinishInputSmallBoard()
        {
            return pack.SetPlcFinishInputSmallBoard();
        }

        public Task<bool> SetPlcFinishOutputLargeBoard()
        {
            return pack.SetPlcFinishOutputLargeBoard();
        }

        public Task<bool> SetPlcFinshInputLargeBoard()
        {
            return pack.SetPlcFinshInputLargeBoard();
        }

        public Task<bool> SetPlcStartOutputSmallBoard()
        {
            return pack.SetPlcStartOutputSmallBoard();
        }

        public Task<bool> SetPlcStartInputLargeBoard()
        {
            return pack.SetPlcStartInputLargeBoard();
        }

        public Task<bool> SetPlcStartInputSmallBoard()
        {
            return pack.SetPlcStartInputSmallBoard();
        }

        public Task<bool> SetPlcStartOutputLargeBoard()
        {
            return pack.SetPlcStartOutputLargeBoard();
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

        public Task<bool> GetPlcOutputSmallBoardStatus()
        {
            return pack.GetPlcOutputSmallBoardStatus();
        }

        public bool IsOutputSmallBoardFinish()
        {
            return pack.IsOutputSmallBoardFinish();
        }

        public Task<bool> SetPlcFinishOutputSmallBoard()
        {
            return pack.SetPlcFinishOutputSmallBoard();
        }

        public Task<bool> GetTablePierTarget()
        {
            return pack.GetTablePierTarget();
        }

        public Task<bool> SetTablePierTarge()
        {
            return pack.SetTablePierTarge();
        }

        public Task UpdateUIPierTable()
        {
            return pack.UpdateUIPierTable();
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
                            if(await GetTablePierTarget())
                            {
                                await UpdateUIPierTable();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if (await SetPlcStartInputLargeBoard())
                            {
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIPierMission();
                                Set(30);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 30:
                            if(await SetLogMissionStart())
                            {
                                await UpdateUIPierLog();
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 40:
                            if(await GetPlcInputLargeBoardStatus())
                            {
                                await UpdateUIPierMission();
                                Set(50);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 50:
                            if(IsInputLargeBoardFinish())
                            {
                                Set(60);
                            }
                            else
                            {
                                Set(40);
                            }
                            break;

                        case 60:
                            if(await SetPlcFinshInputLargeBoard())
                            {
                                Set(70);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 70:
                            if(await SetTableMissionFinsih())
                            {
                                await UpdateUIPierMission();
                                Set(80);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 80:
                            if(await SetTablePierTarge())
                            {
                                await UpdateUIPierTable();
                                Set(90);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 90:
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


                case EPierAction.InputSmallBoard:
                    switch(S3)
                    {
                        case 0:
                            if(await GetTablePierTarget())
                            {
                                await UpdateUIPierTable();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await SetPlcStartInputSmallBoard())
                            {
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIPierMission();
                                Set(30);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 30:
                            if(await SetLogMissionStart())
                            {
                                await UpdateUIPierLog();
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 40:
                            if(await GetPlcInputSmallBoardStatus())
                            {
                                await UpdateUIPierMission();
                                Set(50);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 50:
                            if(IsInputSmallBoardFinish())
                            {
                                Set(60);
                            }
                            else
                            {
                                Set(40);
                            }
                            break;

                        case 60:
                            if(await SetPlcFinishInputSmallBoard())
                            {
                                Set(70);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 70:
                            if(await SetTableMissionFinsih())
                            {
                                await UpdateUIPierMission();
                                Set(80);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 80:
                            if(await SetTablePierTarge())
                            {
                                await UpdateUIPierTable();
                                Set(90);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 90:
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

                case EPierAction.OutputLargeBoard:
                    switch(S3)
                    {
                        case 0:
                            if (await GetTablePierTarget())
                            {
                                await UpdateUIPierTable();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await SetPlcStartOutputLargeBoard())
                            {
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIPierMission();
                                Set(30);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 30:
                            if (await SetLogMissionStart())
                            {
                                await UpdateUIPierLog();
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 40:
                            if(await GetPlcOutputLargeBoardStatus())
                            {
                                await UpdateUIPierMission();
                                Set(50);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 50:
                            if(IsOutputLargeBoardFinish())
                            {
                                Set(60);
                            }
                            else
                            {
                                Set(40);
                            }
                            break;

                        case 60:
                            if(await SetPlcFinishOutputLargeBoard())
                            {
                                Set(70);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 70:
                            if (await SetTableMissionFinsih())
                            {
                                await UpdateUIPierMission();
                                Set(80);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 80:
                            if (await SetTablePierTarge())
                            {
                                await UpdateUIPierTable();
                                Set(90);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 90:
                            if (await SetLogMissionFinish())
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

                case EPierAction.OutputSmallBoard:
                    switch(S3)
                    {
                        case 0:
                            if (await GetTablePierTarget())
                            {
                                await UpdateUIPierTable();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await SetPlcStartOutputSmallBoard())
                            {
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIPierMission();
                                Set(30);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 30:
                            if(await SetLogMissionStart())
                            {
                                await UpdateUIPierLog();
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 40:
                            if(await GetPlcOutputSmallBoardStatus())
                            {
                                await UpdateUIPierMission();
                                Set(50);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 50:
                            if(IsOutputSmallBoardFinish())
                            {
                                Set(60);
                            }
                            else
                            {
                                Set(40);
                            }
                            break;

                        case 60:
                            if(await SetPlcFinishOutputSmallBoard())
                            {
                                Set(70);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 70:
                            if (await SetTableMissionFinsih())
                            {
                                await UpdateUIPierMission();
                                Set(80);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 80:
                            if (await SetTablePierTarge())
                            {
                                await UpdateUIPierTable();
                                Set(90);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 90:
                            if (await SetLogMissionFinish())
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
