using CommonLibraryB_NXP.Tools.LogWritter;
using DevExpress.Blazor;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;
using NXP_Stocker_BlazorProject.MachineModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{

    public partial class MissionAsignDataService : IMissionAsignDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IMissionTableOperate IMissionTableOp;

        readonly INLogWritterObservable INLogWritter;


        public MissionAsignDataService(ILogTableOperate ILogTableOp, IMissionTableOperate IMissionAsignTableOp,
                                       ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
            this.IMissionTableOp = IMissionAsignTableOp;

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
        int _pierNo { get; set; } = 0;

        public int PierNo
        {
            get
            {
                return _pierNo;
            }
            set
            {
                _pierNo = value;
            }
        }

        int _errorCode { get; set; } = 0;

        public int ErrorCode
        {
            get
            {
                return _errorCode;
            }
            set
            {
                _errorCode = value;
            }
        }

        MissionAssignTable _missionAsign { get; set; } = new MissionAssignTable();

        public MissionAssignTable MissionAsign
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

        WarehouseInform _pickPort { get; set; } = new WarehouseInform();

        public WarehouseInform PickPort
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

        WarehouseInform _dropPort { get; set; } = new WarehouseInform();

        public WarehouseInform DropPort
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

        RobotMissionTable _robotMission { get; set; } = new RobotMissionTable();

        public RobotMissionTable RobotMission
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

        List<LogTable> _listMissionAsignLog { get; set; } = new List<LogTable>();

        public List<LogTable> ListMissionAsignLog
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
    }

    public partial class MissionAsignDataService
    {
        public async Task<bool> GetNewMissionAsignTable()
        {
            var result = await IMissionTableOp.GetNewMissionAsign(false, false, false, PierNo);

            if (result.status)
            {
                if(result.table != null)
                {
                    MissionAsign = result.table;
                }
                else
                {
                    MissionAsign = new MissionAssignTable();
                }

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
            var result = await IMissionTableOp.UpSertMission<PierMissionTable>(PierMission);

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
            var result = await IMissionTableOp.UpSertMission<RobotMissionTable>(RobotMission);

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
            var result = await IMissionTableOp.GetMissionById<PierMissionTable>(PierMission.Id);

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
            var result = await IMissionTableOp.GetMissionById<RobotMissionTable>(RobotMission.Id);

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
            var result = await IMissionTableOp.UpSertMissionAsign(MissionAsign);

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

            string equip = "MissionAsign_" + PierNo.ToString();

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


        LogTable _getLogTable(string equip, string logType, string msg)
        {
            LogTable table = new LogTable()
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
