using CommonLibraryB.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using CommonLibraryB_NXP.Library.PLC;

namespace NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage
{

    public partial class RobotTaskPack<EPLC>
    {
        readonly EPLC robot;

        readonly IPlcOperate<EPLC> IRobotOp;

        readonly PlcLibrary<EPLC> RobotLib;

        

        readonly INLogWritterObservable INLogObser;

        public RobotTaskPack(EPLC robot, PlcLibrary<EPLC> robotLib)
        {

        }
    }

    public partial class RobotTaskPack<EPLC>
    {

    }
}
