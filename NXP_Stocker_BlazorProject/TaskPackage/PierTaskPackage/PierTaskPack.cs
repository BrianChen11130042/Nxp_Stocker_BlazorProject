using CommonLibraryB.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using NLog.Targets;
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

        const string info = "Inform";

        const string err = "Error";

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

        public async Task<bool> SetLogMissionStart()
        {
            string temp = dcPierMission[IDataService.PierMission.ActionCode] + "_任務開始";

            if (await IDataService.AddLogByPier(info, temp))
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

        public async Task<bool> SetPlcInputLargeBoard()
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
    }

    #endregion
}
