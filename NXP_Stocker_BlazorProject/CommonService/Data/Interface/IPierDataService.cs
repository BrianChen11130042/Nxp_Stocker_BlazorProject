using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{
    public interface IPierDataService
    {
        string PierName { get; set; }

        PierMissionTable_stub PierMission { get; set; }

        List<LogTable_stub> ListPierLog { get; set; }

        //PierMissionTable
        Task<bool> GetNewPierMissionTable();

        Task<bool> SetPierMissionTable();

        //LogTable
        Task<bool> AddLogByPier(string type, string log);
    }
}
