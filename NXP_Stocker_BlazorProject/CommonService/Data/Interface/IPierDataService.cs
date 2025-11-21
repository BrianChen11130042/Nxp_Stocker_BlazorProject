using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{
    public interface IPierDataService
    {
        int PierNo { get; set; }

        PierMissionTable PierMission { get; set; }

        List<LogTable> ListPierLog { get; set; }

        //PierMissionTable
        Task<bool> GetNewPierMissionTable();

        Task<bool> SetPierMissionTable();

        Task UpdatePierMissionStatusToInQue();

        //LogTable
        Task<bool> AddLogByPier(string type, string log);
    }
}
