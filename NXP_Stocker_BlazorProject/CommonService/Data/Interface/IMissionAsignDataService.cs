using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{
    public interface IMissionAsignDataService
    {
        string PierName { get; set; }

        MissionAsignTable MissionAsign { get; set; }

        WarehouseTable_stub PickPort { get; set; }

        WarehouseTable_stub DropPort { get; set; }

        PierMissionTable PierMission { get; set; }

        RobotMissionTable RobotMission { get; set; }

        List<LogTable_stub> ListMissionAsignLog { get; set; }

        //MissionAsignTable
        Task<bool> GetNewMissionAsignTable();

        Task<bool> SetMissionAsignTable();

        //WarehouseTable
        Task<bool> GetWarehousePickTable();

        Task<bool> GetWarehouseDropTable();

        Task<bool> SetWarehousePickTable();

        Task<bool> SetWarehouseDropTable();

        //PierMissionTable
        Task<bool> SetNewPierMissionTable();

        Task<bool> GetTargetPierMissionTable();

        //RobotMissionTable
        Task<bool> SetNewRobotMissionTable();

        Task<bool> GetTargetRobotMissionTable();

        //LogTable
        Task<bool> AddLogByMissionAsign(string type, string log);
    }
}
