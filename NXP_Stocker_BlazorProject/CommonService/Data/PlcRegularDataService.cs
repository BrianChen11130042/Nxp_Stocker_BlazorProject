using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{
    public partial class PlcRegularDataService : IPlcRegularDataService
    {

        readonly INLogWritterObservable INLogWritter;

        public PlcRegularDataService(ObserverService observerService)
        {
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

    public enum EWhStatus_Stub
    {
        Empty,
        Small,
        Large
    }

    public partial class PlcRegularDataService
    {
        Dictionary<int, EWhStatus_Stub> _dcWarehouse { get; set; } = new Dictionary<int, EWhStatus_Stub>();

        public Dictionary<int, EWhStatus_Stub> DcWarehouse
        {
            get
            {
                return _dcWarehouse;
            }
            set
            {
                _dcWarehouse = value;
            }
        }
    }
}
