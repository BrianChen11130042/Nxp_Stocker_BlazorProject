using CommonLibraryB.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage
{

    public partial class PierTaskPack<EPLC>
    {
        readonly EPLC pier;

        readonly IPlcOperate<EPLC> IPeirOp;

        readonly PlcLibrary<EPLC> pierLib;

        readonly IPierDataService IDataService;
        readonly INLogWritterObservable INLogWritter;

        public PierTaskPack(EPLC pier, PlcLibrary<EPLC> pierLib, 
                            PierDataService dataService, ObserverService observerService)
        {
            this.pier = pier;

            this.IPeirOp = pierLib;

            this.IDataService = dataService;
            this.INLogWritter = observerService;
        }

        async Task writeNLogError(string log)
        {
            await INLogWritter.NotifyNLog(EStatus.Error, log);
        }

        async Task writeNLogInform(string log)
        {
            await INLogWritter.NotifyNLog(EStatus.Info, log);
        }
    }

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
}
