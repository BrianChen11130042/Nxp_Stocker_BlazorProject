using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{

    public partial class RobotDataService : IRobotDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IRobotMissionTableOperate IRobotTableOp;
        readonly INLogWritterObservable INLogWritter;

        public RobotDataService(ILogTableOperate ILogTableOp,
                                IRobotMissionTableOperate IRobotTableOp,
                                ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
            this.IRobotTableOp = IRobotTableOp;
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

    public partial class RobotDataService
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

        RobotMissionTable_stub _robotMission { get; set; } = new RobotMissionTable_stub();

        public RobotMissionTable_stub RobotMission
        {
            get
            {
                return _robotMission;
            }
            set
            {
                value = _robotMission;
            }
        }

        List<LogTable_stub> _listRobotLog { get; set; } = new List<LogTable_stub>();

        public List<LogTable_stub> ListRobotLog
        {
            get
            {
                return _listRobotLog;
            }
            set
            {
                _listRobotLog = value;
            }
        }
    }

    public partial class RobotDataService
    {
        public async Task<bool> GetNewRobotMissionTable()
        {
            var result = await IRobotTableOp.GetNewRobotMission();

            if(result.status)
            {
                if(result.table != null)
                {
                    RobotMission = result.table;
                    PierName = result.table.PierName;
                }
                else
                {
                    RobotMission = new RobotMissionTable_stub();
                    PierName = string.Empty;
                }

                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }

        public async Task<bool> SetRobotMissionTable()
        {
            var result = await IRobotTableOp.UpdateRobotMission(RobotMission);

            if(result.status)
            {
                RobotMission = result.table;
                PierName = result.table.PierName;
                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }
    }

    public partial class RobotDataService
    {
        string _robotLog { get; set; } = string.Empty;

        public async Task<bool> AddLogByRobot(string type, string log)
        {
            if (_robotLog == log)
                return true;
            else
                _robotLog = log;

            string equip = "Robot_" + PierName;

            var table = _getLogTable(equip, type, log);
            var result = await ILogTableOp.AddLogData(table);

            if(result.status)
            {
                ListRobotLog = result.list;
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
