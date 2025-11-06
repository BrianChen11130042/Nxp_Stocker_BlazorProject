using CommonLibraryB.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{

    public partial class MissionAsignDataService : IMissionAsignDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IMissionAsignTableOperate IMissionAsignTableOp;
        readonly IPierMissionTableOperate IPierMissionTableOp;
        readonly IRobotMissionTableOperate IRobotMissionTableOp;
        readonly IWarehouseTableOperate IWarehouseTableOp;

        readonly INLogWritterObservable INLogWritter;


        public MissionAsignDataService(ILogTableOperate ILogTableOp, IMissionAsignTableOperate IMissionAsignTableOp,
                                        IPierMissionTableOperate IPierMissionTableOp, IRobotMissionTableOperate IRobotMissionTableOp,
                                        IWarehouseTableOperate IWarehouseTableOp, ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
            this.IMissionAsignTableOp = IMissionAsignTableOp;
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

    public partial class MissionAsignDataService
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

        WarehouseTable _pickPort { get; set; } = new WarehouseTable();

        public WarehouseTable PickPort
        {
            get
            {
                return _pickPort;
            }
            set
            {
                _pickPort = value;
            }
        }

        WarehouseTable _dropPort { get; set; } = new WarehouseTable();

        public WarehouseTable DropPort
        {
            get
            {
                return _dropPort;
            }
            set
            {
                _dropPort = value;
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

    public partial class MissionAsignDataService
    {
        public async Task<bool> GetNewMissionAsignTable()
        {
            var result = await IMissionAsignTableOp.GetNewMissionAsign(PierName);

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

        public async Task<bool> GetWarehousePickTable()
        {
            var result = await IWarehouseTableOp.GetWHTarget(MissionAsign.PierName, 
                                                             MissionAsign.PickZone, 
                                                             MissionAsign.PickLayer);

            if(result.status)
            {
                PickPort = result.table;

                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }

        public async Task<bool> GetWarehouseDropTable()
        {
            var result = await IWarehouseTableOp.GetWHTarget(MissionAsign.PierName,
                                                             MissionAsign.DropZone,
                                                             MissionAsign.DropLayer);

            if(result.status)
            {
                DropPort = result.table;

                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }

        public async Task<bool> SetNewPierMissionTable()
        {
            var result = await IPierMissionTableOp.AddPierMission(PierMission);

            if(result.status)
            {
                PierMission = result.table;

                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }

        public async Task<bool> SetMissionAsignTable()
        {
            var result = await IMissionAsignTableOp.UpdateMissionAsign(MissionAsign);

            if(result.status)
            {
                MissionAsign = result.table;

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
