using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{
    public interface IMissionAsignDataService
    {
        string PierName { get; set; }

        MissionAsignTable MissionAsign { get; set; }

        WarehouseTable PickPort { get; set; }

        WarehouseTable DropPort { get; set; }

        PierMissionTable PierMission { get; set; }

        Task<bool> GetNewMissionAsignTable();

        Task<bool> SetMissionAsignTable();

        Task<bool> GetWarehousePickTable();

        Task<bool> GetWarehouseDropTable();

        Task<bool> SetNewPierMissionTable();
    }
}
