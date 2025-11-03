using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{
    public interface IPierDataService
    {
        string PierName { get; set; }

        PierMissionTable PierMission { get; set; }

        List<LogTable> ListPierLog { get; set; }

        WarehouseTable PierTable { get; set; }

        //PierMissionTable
        Task<bool> GetNewPierMissionTable();

        Task<bool> SetPierMissionTable();

        //LogTable
        Task<bool> AddLogByPier(string type, string log);

        //WharehouseTable
        Task<bool> GetPierTaget();

        Task<bool> SetPierTaget();
    }
}
