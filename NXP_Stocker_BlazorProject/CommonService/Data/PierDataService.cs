using CommonLibraryB.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{
    public partial class PierDataService : IPierDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IPierMissionTableOperate IPierMissionTableOp;
        readonly IStorageTableOperate IStorageTableOp;
        readonly INLogWritterObservable INLogWritter;

        public PierDataService(ILogTableOperate ILogTableOp,
                               IPierMissionTableOperate IPierMissionTableOp,
                               IStorageTableOperate IStorageTableOp,
                               ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
            this.IPierMissionTableOp = IPierMissionTableOp;
            this.IStorageTableOp = IStorageTableOp;
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


    public partial class PierDataService
    {
        string _pierName { get; set; } = string.Empty;
        public string PierName
        {
            get
            {
                return _pierName;
            }
            set
            {
                _pierName = value;
            }
        }

        PierMissionTable _pierMission { get; set; } = new PierMissionTable();

        public PierMissionTable PierMission
        {
            get
            {
                return _pierMission;
            }
            set
            {
                _pierMission = value;
            }
        }
    }

    public partial class PierDataService
    {
        public async Task<bool> GetNewPierMissionTable()
        {
            var result = await IPierMissionTableOp.GetNewPierMission(PierName);

            if(result.status)
            {
                if(result.table != null)
                {
                    PierMission = result.table;
                }
                else
                {
                    PierMission = new PierMissionTable();
                }

                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }
    }
}
