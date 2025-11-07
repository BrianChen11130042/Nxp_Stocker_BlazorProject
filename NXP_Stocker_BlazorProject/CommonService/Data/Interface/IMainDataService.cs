namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{

    public interface IMainDataService
    {
        //LogTable
        Task<bool> AddLogByMainTask(string type, string log);
    }
}
