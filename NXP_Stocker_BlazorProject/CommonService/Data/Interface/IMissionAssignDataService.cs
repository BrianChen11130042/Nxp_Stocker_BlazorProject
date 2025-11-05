using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{
    public interface IMissionAssignDataService
    {
        string PierName { get; set; }

        MissionAsignTable MissionAsign { get; set; }

        Task<bool> GetNewMissionAsignTable();
    }
}
