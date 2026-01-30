using NXP_Stocker_BlazorProject.MachineModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{
    public interface IUpsRegularDataService
    {
        int UpsNo { get; set; }

        UpsInform UpsInform { get; set; }

    }
}
