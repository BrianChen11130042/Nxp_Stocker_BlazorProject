using CommonLibraryB.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using CommonLibraryB_NXP.Library.PLC;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage
{

    public partial class RobotTaskPack<EPLC>
    {
        readonly EPLC robot;

        readonly IPlcOperate<EPLC> IRobotOp;

        readonly PlcLibrary<EPLC> RobotLib;

        readonly IRobotDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IRobotUIObserverable IRobotObser;

        public RobotTaskPack(EPLC robot, PlcLibrary<EPLC> robotLib,
                             RobotDataService dataService, ObserverService observerService)
        {
            this.robot = robot;
            this.IRobotOp = robotLib;

            this.IDataService = dataService;

            this.INLogObser = observerService;
            this.IRobotObser = observerService;
        }

        async Task writeNLogError(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Error, log);
        }

        async Task writeNLogInform(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Info, log);
        }

        string info { get; set; } = "Inform";

        string err { get; set; } = "Error";

        int robotStatus { get; set; } = 0;
    }

    public partial class RobotTaskPack<EPLC> : IRobotTaskPack
    {

    }
}
