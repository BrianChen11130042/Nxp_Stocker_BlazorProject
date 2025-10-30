using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{
    public interface IPierDataService
    {
        string PierName { get; set; }

        PierMissionTable PierMission { get; set; }

        Task<bool> GetNewPierMissionTable();
    }
}
