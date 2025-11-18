using CommonLibraryB_NXP.Tools.LogWritter;
using DevExpress.Blazor;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;
using NXP_Stocker_BlazorProject.MachineModel;
using System.Reflection.Emit;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{

    public partial class MissionAsignDataService : IMissionAsignDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IMissionAsignTableOperate IMissionAsignTableOp;
        readonly IPierMissionTableOperate IPierMissionTableOp;
        readonly IRobotMissionTableOperate IRobotMissionTableOp;

        readonly INLogWritterObservable INLogWritter;


        public MissionAsignDataService(ILogTableOperate ILogTableOp, IMissionAsignTableOperate IMissionAsignTableOp,
                                       IPierMissionTableOperate IPierMissionTableOp, IRobotMissionTableOperate IRobotMissionTableOp,
                                       ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
            this.IMissionAsignTableOp = IMissionAsignTableOp;
            this.IPierMissionTableOp = IPierMissionTableOp;
            this.IRobotMissionTableOp = IRobotMissionTableOp;

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
            var result = await IMissionAsignTableOp.GetNewMissionAsign(PierNo);

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
