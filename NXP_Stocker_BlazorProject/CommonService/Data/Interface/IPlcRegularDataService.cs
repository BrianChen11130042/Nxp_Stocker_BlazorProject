namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{

    public interface IPlcRegularDataService
    {
        Dictionary<int, EWhStatus_Stub> DcWarehouse { get; set; }
    }
}
