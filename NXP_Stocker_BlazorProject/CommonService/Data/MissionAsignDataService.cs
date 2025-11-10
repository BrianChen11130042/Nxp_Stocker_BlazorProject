using CommonLibraryB_NXP.Tools.LogWritter;
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

        MissionAsignTable_stub _missionAsign { get; set; } = new MissionAsignTable_stub();

        public MissionAsignTable_stub MissionAsign
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

        WarehouseTable_stub _pickPort { get; set; } = new WarehouseTable_stub();

        public WarehouseTable_stub PickPort
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

        WarehouseTable_stub _dropPort { get; set; } = new WarehouseTable_stub();

        public WarehouseTable_stub DropPort
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

        PierMissionTable_stub _pierMission { get; set; } = new PierMissionTable_stub();

        public PierMissionTable_stub PierMission
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

        List<LogTable_stub> _listMissionAsignLog { get; set; } = new List<LogTable_stub>();

        public List<LogTable_stub> ListMissionAsignLog
        {
            get
            {
                return _listMissionAsignLog;
            }
            set
            {
                _listMissionAsignLog = value;
            }
        }

        RobotMissionTable_stub _robotMission { get; set; } = new RobotMissionTable_stub();

        public RobotMissionTable_stub RobotMission
        {
            get
            {
                return _robotMission;
            }
            set
            {
                _robotMission = value;
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
                    MissionAsign = new MissionAsignTable_stub();
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

        public async Task<bool> SetWarehousePickTable()
        {
            var result = await IWarehouseTableOp.SetWHTarget(PickPort);

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

        public async Task<bool> SetWarehouseDropTable()
        {
            var result = await IWarehouseTableOp.SetWHTarget(DropPort);

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

        public async Task<bool> SetNewRobotMissionTable()
        {
            var result = await IRobotMissionTableOp.AddRobotMission(RobotMission);

            if(result.status)
            {
                RobotMission = result.table;

                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }

        public async Task<bool> GetTargetPierMissionTable()
        {
            var result = await IPierMissionTableOp.GetTargetPierMission(PierMission);

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

        public async Task<bool> GetTargetRobotMissionTable()
        {
            var result = await IRobotMissionTableOp.GetTargetRobotMission(RobotMission);

            if(result.status)
            {
                RobotMission = result.table;

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

    public partial class MissionAsignDataService
    {
        string _missionAsignLog { get; set; } = string.Empty;


        public async Task<bool> AddLogByMissionAsign(string type, string log)
        {
            if (_missionAsignLog == log)
                return true;
            else
                _missionAsignLog = log;

            string equip = "MissionAsign_" + PierName;

            var table = _getLogTable(equip, type, log);
            var result = await ILogTableOp.AddLogData(table);

            if(result.status)
            {
                ListMissionAsignLog = result.list;
                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }

        }


        LogTable_stub _getLogTable(string equip, string logType, string msg)
        {
            LogTable_stub table = new LogTable_stub()
            {
                LogType = logType,
                Equipment = equip,
                Msg = msg,
                RecordTime = DateTime.Now,
            };

            return table;
        }
    }
}
