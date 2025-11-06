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

        RobotMissionTable RobotMission { get; set; }

        List<LogTable> ListMissionAsignLog { get; set; }

        //MissionAsignTable
        Task<bool> GetNewMissionAsignTable();

        Task<bool> SetMissionAsignTable();

        //WarehouseTable
        Task<bool> GetWarehousePickTable();

        Task<bool> GetWarehouseDropTable();

        Task<bool> SetWarehousePickTable();

        //PierMissionTable
        Task<bool> SetNewPierMissionTable();

        Task<bool> GetTargetPierMissionTable();

        //RobotMissionTable
        Task<bool> SetNewRobotMissionTable();

        //LogTable
        Task<bool> AddLogByMissionAsign(string type, string log);
    }
}
