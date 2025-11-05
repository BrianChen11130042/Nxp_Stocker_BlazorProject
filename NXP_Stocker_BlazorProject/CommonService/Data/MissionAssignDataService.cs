using CommonLibraryB.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{

    public partial class MissionAssignDataService : IMissionAssignDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IMissionAssignTableOperate IMainMissionTableOp;
        readonly IPierMissionTableOperate IPierMissionTableOp;
        readonly IRobotMissionTableOperate IRobotMissionTableOp;
        readonly IWarehouseTableOperate IWarehouseTableOp;

        readonly INLogWritterObservable INLogWritter;


        public MissionAssignDataService(ILogTableOperate ILogTableOp, IMissionAssignTableOperate IMainMissionTableOp,
                                        IPierMissionTableOperate IPierMissionTableOp, IRobotMissionTableOperate IRobotMissionTableOp,
                                        IWarehouseTableOperate IWarehouseTableOp, ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
            this.IMainMissionTableOp = IMainMissionTableOp;
            this.IPierMissionTableOp = IPierMissionTableOp;
            this.IRobotMissionTableOp = IRobotMissionTableOp;
            this.IWarehouseTableOp = IWarehouseTableOp;

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

    public partial class MissionAssignDataService
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

        MissionAsignTable _missionAsign { get; set; } = new MissionAsignTable();

        public MissionAsignTable MissionAsign
        {
            get
            {
                return _missionAsign;
            }
            set
            {
                _missionAsign = value;
            }
        }


    }

    public partial class MissionAssignDataService
    {
        public async Task<bool> GetNewMissionAsignTable()
        {
            var result = await IMainMissionTableOp.GetNewMainMission(PierName);

            if (result.status)
            {
                if(result.table != null)
                {
                    MissionAsign = result.table;
                }
                else
                {
                    MissionAsign = new MissionAsignTable();
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
