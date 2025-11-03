using CommonLibraryB.Tools.LogWritter;
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

        RobotMissionTable _robotMission { get; set; } = new RobotMissionTable();

        public RobotMissionTable RobotMission
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
                    RobotMission = new RobotMissionTable();
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
    }
}
