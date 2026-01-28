using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.MachineModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data
{

    public partial class UpsRegularDataService : IUpsRegularDataService
    {

        readonly INLogWritterObservable INLogWritter;

        public UpsRegularDataService(ObserverService observerService)
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

    public partial class UpsRegularDataService
    {
        UpsInform _upsInform { get; set; } = new UpsInform();

        public UpsInform UpsInform
        {
            get
            {
                return _upsInform;
            }
            set
            {
                _upsInform = value;
            }
        }
    }
}
