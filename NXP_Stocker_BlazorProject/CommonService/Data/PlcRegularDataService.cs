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

    public enum EWhStatus
    {
        Empty,
        Small,
        Large
    }

    public partial class PlcRegularDataService
    {
        Dictionary<int, EWhStatus> _dcWarehouse { get; set; } = new Dictionary<int, EWhStatus>();

        public Dictionary<int, EWhStatus> DcWarehouse
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
