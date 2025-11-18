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
        readonly INLogWritterObservable INLogWritter;

        public MainDataService(ILogTableOperate ILogTableOp, ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
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


        List<LogTable_stub> _listMainLog { get; set; } = new List<LogTable_stub>();

        public List<LogTable_stub> ListMainLog
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
