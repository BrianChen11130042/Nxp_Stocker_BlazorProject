using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Adapter;

namespace NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage
{

    public partial class PierTaskPack<EPLC>
    {
        readonly EPLC pier;

        readonly IPlcOperate<EPLC> IPeirOp;

        readonly PlcLibrary<EPLC> pierLib;

        public PierTaskPack()
        {

        }

    }
}
