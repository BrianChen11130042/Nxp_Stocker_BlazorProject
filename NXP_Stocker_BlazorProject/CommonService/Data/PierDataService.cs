using CommonLibraryB.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{
    public partial class PierDataService : IPierDataService
    {
        readonly ILogTableOperate ILogTableOp;
        readonly IPierMissionTableOperate IPierMissionTableOp;
        readonly IWarehouseTableOperate IWarehouseTableOp;
        readonly INLogWritterObservable INLogWritter;

        public PierDataService(ILogTableOperate ILogTableOp,
                               IPierMissionTableOperate IPierMissionTableOp,
                               IWarehouseTableOperate IWarehouseTableOp,
                               ObserverService observerService)
        {
            this.ILogTableOp = ILogTableOp;
            this.IPierMissionTableOp = IPierMissionTableOp;
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

        WarehouseTable _pierTable { get; set; } = new WarehouseTable();

        public WarehouseTable PierTable
        {
            get
            {
                return _pierTable;
            }
            set
            {
                _pierTable = value;
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
        public async Task<bool> GetPierTaget()
        {
            int sizeSpec = getBoardSize(PierMission.ActionCode);
            bool occupy = getIsOccupy(PierMission.ActionCode);

            var result = await IWarehouseTableOp.GetPierTarget(PierMission.PierName, sizeSpec, occupy);

            if(result.status)
            {
                PierTable = result.table;
                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }

        int getBoardSize(int actionCode)
        {
            switch(actionCode)
            {
                case 1:
                case 2:
                    return 0;

                case 3:
                case 4:
                    return 1;

                default:
                    return 2;
            }
        }

        bool getIsOccupy(int actionCode)
        {
            switch (actionCode)
            {
                case 1:
                case 3:
                    return false;

                case 2:
                case 4:
                    return true;

                default:
                    return false;
            }
        }

        public async Task<bool> SetPierTaget()
        {
            SetPierTable(PierMission.ActionCode);

            var result = await IWarehouseTableOp.SetPierTarget(PierTable);

            if (result.status)
            {
                PierTable = result.table;
                return result.status;
            }
            else
            {
                await writeNLogError(result.msg);
                return result.status;
            }
        }

        void SetPierTable(int actionCode)
        {
            switch(actionCode)
            {
                case 1:
                    PierTable.IsOccupy = true;
                    PierTable.Barcode = PierMission.Barcode;
                    PierTable.BoardSize = 0;
                    break;

                case 2:
                    PierTable.IsOccupy = false;
                    PierTable.Barcode = string.Empty;
                    PierTable.BoardSize = 999;
                    break;

                case 3:
                    PierTable.IsOccupy = true;
                    PierTable.Barcode = PierMission.Barcode;
                    PierTable.BoardSize = 1;
                    break;

                case 4:
                    PierTable.IsOccupy = false;
                    PierTable.Barcode = string.Empty;
                    PierTable.BoardSize = 999;
                    break;
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
