using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{
    public partial class PierDataService : IPierDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IPierMissionTableOperate IPierMissionTableOp;
        readonly INLogWritterObservable INLogWritter;

        public PierDataService(ILogTableOperate ILogTableOp,
                               IPierMissionTableOperate IPierMissionTableOp,
                               ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
            this.IPierMissionTableOp = IPierMissionTableOp;
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

        List<LogTable> _listPierLog { get; set; } = new List<LogTable>();

        public List<LogTable> ListPierLog
        {
            get
            {
                return _listPierLog;
            }
            set
            {
                _listPierLog = value;
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

        public async Task<bool> SetPierMissionTable()
        {
            var result = await IPierMissionTableOp.UpdatePierMission(PierMission);

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
    }

    public partial class PierDataService
    {
        string _pierLog { get; set; } = string.Empty; 

        public async Task<bool> AddLogByPier(string type, string log)
        {
            if (_pierLog == log)
                return true;
            else
                _pierLog = log;

            var table = _getLogTable(PierName, type, log);
            var result = await ILogTableOp.AddLogData(table);

            if(result.status)
            {
                ListPierLog = result.list;
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
