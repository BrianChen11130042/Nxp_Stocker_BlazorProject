using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{

    public partial class PierTask : IPierTaskPack
    {

        readonly IPierTaskPack pack;

        public PierTask(IPierTaskPack pack)
        {
            this.pack = pack;
            interval = 1;
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

        public Task<bool> GetPlcPierNo()
        {
            return pack.GetPlcPierNo();
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

        public Task UpdatePierMissionStatusToInque()
        {
            return pack.UpdatePierMissionStatusToInque();
        }

        public Task<bool> GetPlcIsReady()
        {
            return pack.GetPlcIsReady();
        }

        public bool IsPlcReady()
        {
            return pack.IsPlcReady();
        }

        public Task UpdateUIPierStop()
        {
            return pack.UpdateUIPierStop();
        }

        public Task UpdateUIPierIdle()
        {
            return pack.UpdateUIPierIdle();
        }

        public Task UpdateUIPierRunning()
        {
            return pack.UpdateUIPierRunning();
        }

        public Task UpdateUIPierMotionStatus()
        {
            return pack.UpdateUIPierMotionStatus();
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
                            await GetPlcPierNo();

                            if (await GetPlcIsReady())
                            {
                                if(IsPlcReady())
                                {
                                    Set(10);
                                }
                                else
                                {
                                    await UpdateUIPierStop();

                                    Set(0);
                                }
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

                                if (IsInputLargeBoard())
                                {

                                    if (await SetPlcStartInputLargeBoard())
                                    {
                                        await UpdateUIPierRunning();

                                        Set(EPierAction.InputLargeBoard, 0);
                                    }
                                    else
                                    {
                                        SaveState();
                                        Set(ES1.Error, EPierAction.None, 0);
                                    }

                                }
                                else if (IsInputSmallBoard())
                                {

                                    if (await SetPlcStartInputSmallBoard())
                                    {
                                        await UpdateUIPierRunning();

                                        Set(EPierAction.InputSmallBoard, 0);
                                    }
                                    else
                                    {
                                        SaveState();
                                        Set(ES1.Error, EPierAction.None, 0);
                                    }

                                }
                                else if (IsOutputLargeBoard())
                                {

                                    if (await SetPlcStartOutputLargeBoard())
                                    {
                                        await UpdateUIPierRunning();

                                        Set(EPierAction.OutputLargeBoard, 0);
                                    }
                                    else
                                    {
                                        SaveState();
                                        Set(ES1.Error, EPierAction.None, 0);
                                    }

                                }
                                else if (IsOutputSmallBoard())
                                {

                                    if (await SetPlcStartOutputSmallBoard())
                                    {
                                        await UpdateUIPierRunning();

                                        Set(EPierAction.OutputSmallBoard, 0);
                                    }
                                    else
                                    {
                                        SaveState();
                                        Set(ES1.Error, EPierAction.None, 0);
                                    }

                                }
                                else
                                {
                                    await UpdateUIPierIdle();

                                    Set(0);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;
                    }
                    break;

                case EPierAction.InputLargeBoard:
                    switch (S3)
                    {
                        case 0:
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIPierMission();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await SetLogMissionStart())
                            {
                                await UpdateUIPierLog();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await GetPlcInputLargeBoardStatus())
                            {
                                await UpdatePierMissionStatusToInque();
                                await UpdateUIPierMission();
                                await UpdateUIPierMotionStatus();

                                if (IsInputLargeBoardFinish())
                                {

                                    if (await SetTableMissionFinsih())
                                    {
                                        await UpdateUIPierStop();
                                        await UpdateUIPierMission();
                                        Set(30);
                                    }
                                    else
                                    {
                                        SaveState();
                                        Set(ES1.Error, EPierAction.None, 0);
                                    }

                                }
                                else
                                {
                                    Set(20);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 30:
                            if(await SetPlcFinshInputLargeBoard())
                            {
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 40:
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
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIPierMission();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await SetLogMissionStart())
                            {
                                await UpdateUIPierLog();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await GetPlcInputSmallBoardStatus())
                            {
                                await UpdatePierMissionStatusToInque();
                                await UpdateUIPierMission();
                                await UpdateUIPierMotionStatus();

                                if (IsInputSmallBoardFinish())
                                {

                                    if (await SetTableMissionFinsih())
                                    {
                                        await UpdateUIPierStop();
                                        await UpdateUIPierMission();
                                        Set(30);
                                    }
                                    else
                                    {
                                        SaveState();
                                        Set(ES1.Error, EPierAction.None, 0);
                                    }

                                }
                                else
                                {
                                    Set(20);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 30:
                            if(await SetPlcFinishInputSmallBoard())
                            {
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 40:
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
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIPierMission();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if (await SetLogMissionStart())
                            {
                                await UpdateUIPierLog();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await GetPlcOutputLargeBoardStatus())
                            {
                                await UpdatePierMissionStatusToInque();
                                await UpdateUIPierMission();
                                await UpdateUIPierMotionStatus();

                                if (IsOutputLargeBoardFinish())
                                {

                                    if (await SetTableMissionFinsih())
                                    {
                                        await UpdateUIPierStop();
                                        await UpdateUIPierMission();
                                        Set(30);
                                    }
                                    else
                                    {
                                        SaveState();
                                        Set(ES1.Error, EPierAction.None, 0);
                                    }

                                }
                                else
                                {
                                    Set(20);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 30:
                            if(await SetPlcFinishOutputLargeBoard())
                            {
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 40:
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
                            if(await SetTableMissionStart())
                            {
                                await UpdateUIPierMission();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 10:
                            if(await SetLogMissionStart())
                            {
                                await UpdateUIPierLog();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 20:
                            if(await GetPlcOutputSmallBoardStatus())
                            {
                                await UpdatePierMissionStatusToInque();
                                await UpdateUIPierMission();
                                await UpdateUIPierMotionStatus();

                                if (IsOutputSmallBoardFinish())
                                {

                                    if (await SetTableMissionFinsih())
                                    {
                                        await UpdateUIPierStop();
                                        await UpdateUIPierMission();
                                        Set(30);
                                    }
                                    else
                                    {
                                        SaveState();
                                        Set(ES1.Error, EPierAction.None, 0);
                                    }

                                }
                                else
                                {
                                    Set(20);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 30:
                            if(await SetPlcFinishOutputSmallBoard())
                            {
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPierAction.None, 0);
                            }
                            break;

                        case 40:
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
