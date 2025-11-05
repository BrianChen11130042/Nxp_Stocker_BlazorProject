using CommonLibraryB.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage
{
    public partial class MissionAsignTaskPack<EPLC>
    {
        readonly EPLC pier;

        readonly IPlcOperate<EPLC> IPeirOp;

        readonly PlcLibrary<EPLC> pierLib;

        readonly IMissionAssignDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IMissionAssignUIObserverable IMissionAsignObser;

        public MissionAsignTaskPack(EPLC pier, PlcLibrary<EPLC> pierLib,
                                    MissionAssignDataService dataService, ObserverService observerService)
        {
            this.pier = pier;
            this.IPeirOp = pierLib;
            this.pierLib = pierLib;

            this.IDataService = dataService;
            this.INLogObser = observerService;
            this.IMissionAsignObser = observerService;
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

        Dictionary<int, string> dcMissionAssign { get; set; } = new Dictionary<int, string>()
        {
            { 0, "NoData"},
            { 1, "入庫"},
            { 2, "出庫"},
            { 3, "儲位轉移"}
        };
    }

    public partial class MissionAsignTaskPack<EPLC> : IMissionAsignTaskPack
    {
        public async Task<bool> GetPlcPierName()
        {
            if (await IPeirOp.GetDeviceName(pier))
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

        public async Task<bool> GetTableNewMissionAsign()
        {
            if(await IDataService.GetNewMissionAsignTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateUIMissionAsign()
        {
            await IMissionAsignObser.NotifyMissionAsign(IDataService.PierName, IDataService.MissionAsign);
        }
    }

    public partial class MissionAsignTaskPack<EPLC>
    {
        public bool IsInputWarehouse()
        {
            if (!string.IsNullOrEmpty(IDataService.MissionAsign.MissionSerialNumber)
                && !string.IsNullOrEmpty(IDataService.MissionAsign.Barcode)
                && IDataService.MissionAsign.ActionCode == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public partial class MissionAsignTaskPack<EPLC>
    {
        public bool IsOutputWarehouse()
        {
            if(!string.IsNullOrEmpty(IDataService.MissionAsign.MissionSerialNumber)
                && !string.IsNullOrEmpty(IDataService.MissionAsign.Barcode)
                && IDataService.MissionAsign.ActionCode == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public partial class MissionAsignTaskPack<EPLC>
    {
        public bool IsTransformWarehouse()
        {
            if(!string.IsNullOrEmpty(IDataService.MissionAsign.MissionSerialNumber)
                && !string.IsNullOrEmpty(IDataService.MissionAsign.Barcode)
                && IDataService.MissionAsign.ActionCode == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
