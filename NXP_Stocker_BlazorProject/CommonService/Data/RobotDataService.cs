using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{

    public partial class RobotDataService : IRobotDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IMissionTableOperate IMissionTableOp;
        readonly INLogWritterObservable INLogWritter;

        public RobotDataService(ILogTableOperate ILogTableOp,
                                IMissionTableOperate IMissionTableOp,
                                ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
            this.IMissionTableOp = IMissionTableOp;
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
        int _robotNo { get; set; } = 0;

        public int RobotNo
        {
            get
            {
                return _robotNo;
            }
            set
            {
                _robotNo = value;
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

        List<LogTable> _listRobotLog { get; set; } = new List<LogTable>();

        public List<LogTable> ListRobotLog
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
            var result = await IMissionTableOp.GetNewMission<RobotMissionTable>(false, false);

            if(result.status)
            {
                if(result.table != null)
                {
                    RobotMission = result.table;
                }
                else
                {
                    RobotMission = new RobotMissionTable();
                    RobotMission.PierNo = 0;
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

        public async Task UpdateRobotMissionStatusToInque()
        {
            await IMissionTableOp.UpdateMissionStatusToInQue<RobotMissionTable>(RobotMission);
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

            string equip = "Robot_Pier" + RobotMission.PierNo.ToString();

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
