using CommonLibraryB_NXP.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage
{

    public partial class PierTaskPack<EPLC>
    {
        readonly EPLC pier;

        readonly IPlcOperate<EPLC> IPlcOp;

        readonly PlcLibrary<EPLC> plcLib;

        readonly IPierDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IPierUIObserverable IPierObser;

        public PierTaskPack(EPLC pier, PlcLibrary<EPLC> plcLib,
                            PierDataService dataService, ObserverService observerService)
        {
            this.pier = pier;
            this.IPlcOp = plcLib;
            this.plcLib = plcLib;

            this.IDataService = dataService;

            this.INLogObser = observerService;
            this.IPierObser = observerService;
        }

        async Task writeNLogError(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Error, log);
        }

        async Task writeNLogInform(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Info, log);
        }

        string info { get; set; } = "Inform";

        string err { get; set; } = "Error";

        Dictionary<int, string> dcPierMission { get; set; } = new Dictionary<int, string>()
        {
            { 0, "NoData"},
            { 1, "入大板"},
            { 2, "出大板"},
            { 3, "入小板"},
            { 4, "出小板"}
        };
    }

    #region Common

    public partial class PierTaskPack<EPLC> : IPierTaskPack
    {
        public async Task<bool> GetPlcPierNo()
        {
            if(await IPlcOp.GetDeviceNo(pier))
            {
                IDataService.PierNo = plcLib.Packages[pier].property.getPier.pierNo;
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        int _reset { get; set; } = 0;

        public async Task<bool> GetPlcIsReady()
        {
            if (await IPlcOp.GetDeviceIsReady(pier))
            {
                _reset = plcLib.Packages[pier].property.getPier.isReady;
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsPlcReady()
        {
            if (_reset == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> GetPlcIsPierError()
        {
            if(await IPlcOp.GetDeviceIsError(pier))
            {
                IDataService.PierMission.Status = plcLib.Packages[pier].property.getPier.errorCode;
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsPierError()
        {
            if(IDataService.PierMission.Status != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> GetTableNewMission()
        {
            if(await IDataService.GetNewPierMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTableMissionStart()
        {
            //IDataService.PierMission.IsStart = true;
            IDataService.PierMission.StartTime = DateTime.Now;

            if(await IDataService.SetPierMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTableMissionError()
        {
            int _errorCode = IDataService.PierMission.Status;
            IDataService.PierMission.ErrorCode = _errorCode;

            if (await IDataService.SetPierMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTableMissionFinsih()
        {
            //IDataService.PierMission.IsFinish = true;
            IDataService.PierMission.FinishTime = DateTime.Now;

            if(await IDataService.SetPierMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetLogMissionStart()
        {
            string temp = "Pier" + IDataService.PierNo.ToString() + dcPierMission[IDataService.PierMission.ActionCode] + "_任務開始";

            if (await IDataService.AddLogByPier(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetLogMissionError()
        {
            string temp = "Pier" + IDataService.PierNo.ToString() + dcPierMission[IDataService.PierMission.ActionCode] + "_任務異常，錯誤碼：" + 
                           IDataService.PierMission.ErrorCode.ToString();

            if (await IDataService.AddLogByPier(err, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetLogMissionFinish()
        {
            string temp = "Pier" + IDataService.PierNo.ToString() + dcPierMission[IDataService.PierMission.ActionCode] + "_任務結束";

            if(await IDataService.AddLogByPier(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdatePierMissionStatusToInque()
        {
            await IDataService.UpdatePierMissionStatusToInQue();
        }

        public async Task UpdateUIPierMission()
        {
            await IPierObser.NotifyPierMission(IDataService.PierNo, IDataService.PierMission);
        }

        public async Task UpdateUIPierLog()
        {
            await IPierObser.NotifyPierLog(IDataService.PierNo, IDataService.ListPierLog);
        }

        public async Task UpdateUIPierDisconnect()
        {
            await IPierObser.NotifyPierAction(IDataService.PierNo, 902);
        }

        public async Task UpdateUIPierStop()
        {
            await IPierObser.NotifyPierAction(IDataService.PierNo, 907);
        }

        public async Task UpdateUIPierIdle()
        {
            await IPierObser.NotifyPierAction(IDataService.PierNo, 904);
        }

        public async Task UpdateUIPierRunning()
        {
            await IPierObser.NotifyPierAction(IDataService.PierNo, 905);
        }

        public async Task UpdateUIPierMotionStatus()
        {
            await IPierObser.NotifyPierAction(IDataService.PierNo, IDataService.PierMission.Status);
        }
    }

    #endregion

    #region Input LargeBoard

    public partial class PierTaskPack<EPLC>
    {
        public bool IsInputLargeBoard()
        {
            if (!string.IsNullOrEmpty(IDataService.PierMission.Barcode)
                && IDataService.PierMission.ActionCode == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcStartInputLargeBoard()
        {
            plcLib.Packages[pier].property.setPier.missionStart = (ushort)IDataService.PierMission.ActionCode;

            if (await IPlcOp.SetPierMissionStart(pier))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> GetPlcInputLargeBoardStatus()
        {
            if(await IPlcOp.GetPierStatus(pier))
            {
                IDataService.PierMission.Status = plcLib.Packages[pier].property.getPier.missionStatus;
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsInputLargeBoardFinish()
        {
            if(IDataService.PierMission.Status == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcFinshInputLargeBoard()
        {
            plcLib.Packages[pier].property.setPier.missionFinish = 100;

            if(await IPlcOp.SetPierMissionFinish(pier))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }
    }

    #endregion

    #region Input SmallBoard

    public partial class PierTaskPack<EPLC>
    {
        public bool IsInputSmallBoard()
        {
            if (!string.IsNullOrEmpty(IDataService.PierMission.Barcode)
                && IDataService.PierMission.ActionCode == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcStartInputSmallBoard()
        {
            plcLib.Packages[pier].property.setPier.missionStart = (ushort)IDataService.PierMission.ActionCode;

            if (await IPlcOp.SetPierMissionStart(pier))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> GetPlcInputSmallBoardStatus()
        {
            if(await IPlcOp.GetPierStatus(pier))
            {
                IDataService.PierMission.Status = plcLib.Packages[pier].property.getPier.missionStatus;
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsInputSmallBoardFinish()
        {
            if(IDataService.PierMission.Status == 25)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcFinishInputSmallBoard()
        {
            plcLib.Packages[pier].property.setPier.missionFinish = 100;

            if (await IPlcOp.SetPierMissionFinish(pier))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

    }

    #endregion

    #region Output LargeBoard

    public partial class PierTaskPack<EPLC>
    {
        public bool IsOutputLargeBoard()
        {
            if (!string.IsNullOrEmpty(IDataService.PierMission.Barcode)
                && IDataService.PierMission.ActionCode == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcStartOutputLargeBoard()
        {
            plcLib.Packages[pier].property.setPier.missionStart = (ushort)IDataService.PierMission.ActionCode;

            if (await IPlcOp.SetPierMissionStart(pier))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> GetPlcOutputLargeBoardStatus()
        {
            if (await IPlcOp.GetPierStatus(pier))
            {
                IDataService.PierMission.Status = plcLib.Packages[pier].property.getPier.missionStatus;
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsOutputLargeBoardFinish()
        {
            if(IDataService.PierMission.Status == 15)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcFinishOutputLargeBoard()
        {
            plcLib.Packages[pier].property.setPier.missionFinish = 100;

            if (await IPlcOp.SetPierMissionFinish(pier))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }
    }

    #endregion

    #region Output SmallBoard

    public partial class PierTaskPack<EPLC>
    {
        public bool IsOutputSmallBoard()
        {
            if (!string.IsNullOrEmpty(IDataService.PierMission.Barcode)
                && IDataService.PierMission.ActionCode == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcStartOutputSmallBoard()
        {
            plcLib.Packages[pier].property.setPier.missionStart = (ushort)IDataService.PierMission.ActionCode;

            if (await IPlcOp.SetPierMissionStart(pier))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> GetPlcOutputSmallBoardStatus()
        {
            if (await IPlcOp.GetPierStatus(pier))
            {
                IDataService.PierMission.Status = plcLib.Packages[pier].property.getPier.missionStatus;
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsOutputSmallBoardFinish()
        {
            if(IDataService.PierMission.Status == 35)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcFinishOutputSmallBoard()
        {
            plcLib.Packages[pier].property.setPier.missionFinish = 100;

            if (await IPlcOp.SetPierMissionFinish(pier))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }
    }

    #endregion
}
