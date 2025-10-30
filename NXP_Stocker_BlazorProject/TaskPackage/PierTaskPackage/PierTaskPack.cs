using CommonLibraryB.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;

namespace NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage
{

    public partial class PierTaskPack<EPLC>
    {
        readonly EPLC pier;

        readonly IPlcOperate<EPLC> IPeirOp;

        readonly PlcLibrary<EPLC> pierLib;

        readonly IDataService IDataService;
        readonly INLogWritterObservable INLogWritter;

        public PierTaskPack(EPLC pier, PlcLibrary<EPLC> pierLib, 
                            DataService dataService, ObserverService observerService)
        {
            this.pier = pier;

            this.IPeirOp = pierLib;

            this.IDataService = dataService;
            this.INLogWritter = observerService;
        }

    }
}
