using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{
    public partial class MainDataService : IMainDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IMissionTableOperate IMissionTableOp;
        readonly INLogWritterObservable INLogWritter;

        public MainDataService(ILogTableOperate ILogTableOp, IMissionTableOperate IMissionTableOp,
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

    public partial class MainDataService
    {
        bool _isModbusConnect { get; set; } = false;

        public bool IsModbusConnect
        {
            get
            {
                return _isModbusConnect;
            }
            set
            {
                _isModbusConnect = value;
            }
        }

        int _pier1No { get; set; } = 0;

        public int Pier1No
        {
            get
            {
                return _pier1No;
            }
            set
            {
                _pier1No = value;
            }
        }

        int _pier2No { get; set; } = 0;

        public int Pier2No
        {
            get
            {
                return _pier2No;
            }
            set
            {
                _pier2No = value;
            }
        }

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

        int _upsNo { get; set; } = 0;

        public int UpsNo
        {
            get
            {
                return _upsNo;
            }
            set
            {
                _upsNo = value;
            }
        }

        List<LogTable> _listMainLog { get; set; } = new List<LogTable>();

        public List<LogTable> ListMainLog
        {
            get
            {
                return _listMainLog;
            }
            set
            {
                _listMainLog = value;
            }
        }
    }

    public partial class MainDataService
    {
        public async Task<bool> InitMissionAsignByInQue()
        {
            var result = await IMissionTableOp.InitMissionAsignToInQue();

            if(result.status)
            {
                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }
    }

    public partial class MainDataService
    {
        string _mainLog { get; set; } = string.Empty;


        public async Task<bool> AddLogByMainTask(string type, string log)
        {
            if (_mainLog == log)
                return true;
            else
                _mainLog = log;

            string equip = "System";

            var table = _getLogTable(equip, type, log);
            var result = await ILogTableOp.AddLogData(table);

            if (result.status)
            {
                ListMainLog = result.list;
                return result.status;
            }
            else
            {
                ListMainLog.Add(table);
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
