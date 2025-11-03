using CommonLibraryB.Tools.LogWritter;
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

        readonly IPlcOperate<EPLC> IPeirOp;

        readonly PlcLibrary<EPLC> pierLib;

        readonly IPierDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IPierUIObserverable IPierObser;

        public PierTaskPack(EPLC pier, PlcLibrary<EPLC> pierLib,
                            PierDataService dataService, ObserverService observerService)
        {
            this.pier = pier;
            this.IPeirOp = pierLib;

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

        int pierStatus { get; set; } = 0;

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
        public async Task<bool> GetPlcPierName()
        {
            if(await IPeirOp.GetDeviceName(pier))
            {
                IDataService.PierName = pierLib.Packages[pier].property.getPier.pierName;
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
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

        public async Task<bool> GetTablePierTarget()
        {
            if (await IDataService.GetPierTaget())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTablePierTarge()
        {
            if(await IDataService.SetPierTaget())
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
            IDataService.PierMission.IsStart = true;
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

        public async Task<bool> SetTableMissionFinsih()
        {
            IDataService.PierMission.IsFinish = true;
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
            string temp = IDataService.PierName + dcPierMission[IDataService.PierMission.ActionCode] + "_任務開始";

            if (await IDataService.AddLogByPier(info, temp))
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
            string temp = IDataService.PierName + dcPierMission[IDataService.PierMission.ActionCode] + "_任務結束";

            if(await IDataService.AddLogByPier(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateUIPierMission()
        {
            await IPierObser.NotifyPierMission(IDataService.PierName, IDataService.PierMission);
        }

        public async Task UpdateUIPierLog()
        {
            await IPierObser.NotifyPierLog(IDataService.PierName, IDataService.ListPierLog);
        }

        public async Task UpdateUIPierTable()
        {
            await IPierObser.NotifyPierPortTable(IDataService.PierName, IDataService.PierTable);
        }
    }

    #endregion

    #region Input LargeBoard

    public partial class PierTaskPack<EPLC>
    {
        public bool IsInputLargeBoard()
        {
            if (!string.IsNullOrEmpty(IDataService.PierMission.MissionSerialNumber)
                && !string.IsNullOrEmpty(IDataService.PierMission.Barcode)
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
            pierLib.Packages[pier].property.setPier.missionStart = (ushort)IDataService.PierMission.ActionCode;

            if (await IPeirOp.SetPierMissionStart(pier))
            {
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        Dictionary<int, string> dcInputLargeBoard { get; set; } = new Dictionary<int, string>()
        {
            { 0, "NO DATA"},
            { 1, "等人按鈕1"},
            { 100, "Shuttle伸出條件不滿足"},
            { 2, "Shuttle伸出中"},
            { 3, "等人按鈕2"},
            { 102, "Shuttle收回條件不滿足"},
            { 4, "Shuttle收回中"},
            { 5, "入大板完成"}
        };

        public async Task<bool> GetPlcInputLargeBoardStatus()
        {
            if(await IPeirOp.GetPierStatus(pier))
            {
                pierStatus = pierLib.Packages[pier].property.getPier.missionStatus;
                IDataService.PierMission.StepStatus = dcInputLargeBoard[pierStatus];
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsInputLargeBoardFinish()
        {
            if(pierStatus == 5)
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
            pierLib.Packages[pier].property.setPier.missionFinish = 0;

            if(await IPeirOp.SetPierMissionFinish(pier))
            {
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
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
            if (!string.IsNullOrEmpty(IDataService.PierMission.MissionSerialNumber)
                && !string.IsNullOrEmpty(IDataService.PierMission.Barcode)
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
            pierLib.Packages[pier].property.setPier.missionStart = (ushort)IDataService.PierMission.ActionCode;

            if (await IPeirOp.SetPierMissionStart(pier))
            {
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        Dictionary<int, string> dcInputSmallBoard { get; set; } = new Dictionary<int, string>()
        {
            { 0, "NO DATA"},
            { 21, "等人按鈕1"},
            { 100, "Shuttle伸出條件不滿足"},
            { 22, "Shuttle伸出中"},
            { 23, "等人按鈕2"},
            { 104, "Shuttle收回條件不滿足"},
            { 24, "Shuttle收回中"},
            { 25, "入小板完成"}
        };

        public async Task<bool> GetPlcInputSmallBoardStatus()
        {
            if(await IPeirOp.GetPierStatus(pier))
            {
                pierStatus = pierLib.Packages[pier].property.getPier.missionStatus;
                IDataService.PierMission.StepStatus = dcInputSmallBoard[pierStatus];
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsInputSmallBoardFinish()
        {
            if(pierStatus == 25)
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
            pierLib.Packages[pier].property.setPier.missionFinish = 0;

            if (await IPeirOp.SetPierMissionFinish(pier))
            {
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
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
            if (!string.IsNullOrEmpty(IDataService.PierMission.MissionSerialNumber)
                && !string.IsNullOrEmpty(IDataService.PierMission.Barcode)
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
            pierLib.Packages[pier].property.setPier.missionStart = (ushort)IDataService.PierMission.ActionCode;

            if (await IPeirOp.SetPierMissionStart(pier))
            {
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        Dictionary<int, string> dcOutputLargeBoard { get; set; } = new Dictionary<int, string>()
        {
            { 0, "NO DATA"},
            { 11, "等人按鈕1"},
            { 106, "Shuttle伸出條件不滿足"},
            { 12, "Shuttle伸出中"},
            { 13, "等人按鈕2"},
            { 110, "Shuttle收回條件不滿足"},
            { 14, "Shuttle收回中"},
            { 15, "出大板完成"}
        };

        public async Task<bool> GetPlcOutputLargeBoardStatus()
        {
            if (await IPeirOp.GetPierStatus(pier))
            {
                pierStatus = pierLib.Packages[pier].property.getPier.missionStatus;
                IDataService.PierMission.StepStatus = dcOutputLargeBoard[pierStatus];
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsOutputLargeBoardFinish()
        {
            if(pierStatus == 15)
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
            pierLib.Packages[pier].property.setPier.missionFinish = 0;

            if (await IPeirOp.SetPierMissionFinish(pier))
            {
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
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
            if (!string.IsNullOrEmpty(IDataService.PierMission.MissionSerialNumber)
                && !string.IsNullOrEmpty(IDataService.PierMission.Barcode)
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
            pierLib.Packages[pier].property.setPier.missionStart = (ushort)IDataService.PierMission.ActionCode;

            if (await IPeirOp.SetPierMissionStart(pier))
            {
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        Dictionary<int, string> dcOutputSmallBoard { get; set; } = new Dictionary<int, string>()
        {
            { 0, "NO DATA"},
            { 31, "等人按鈕1"},
            { 108, "Shuttle伸出條件不滿足"},
            { 32, "Shuttle伸出中"},
            { 33, "等人按鈕2"},
            { 105, "Shuttle收回條件不滿足"},
            { 34, "Shuttle收回中"},
            { 35, "出大板完成"}
        };

        public async Task<bool> GetPlcOutputSmallBoardStatus()
        {
            if (await IPeirOp.GetPierStatus(pier))
            {
                pierStatus = pierLib.Packages[pier].property.getPier.missionStatus;
                IDataService.PierMission.StepStatus = dcOutputSmallBoard[pierStatus];
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsOutputSmallBoardFinish()
        {
            if(pierStatus == 35)
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
            pierLib.Packages[pier].property.setPier.missionFinish = 0;

            if (await IPeirOp.SetPierMissionFinish(pier))
            {
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }
    }

    #endregion
}
