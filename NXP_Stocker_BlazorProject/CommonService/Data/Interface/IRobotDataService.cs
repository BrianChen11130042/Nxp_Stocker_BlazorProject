using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{

    public interface IRobotDataService
    {
        string PierName { get; set; }

        RobotMissionTable RobotMission { get; set; }

        List<LogTable> ListRobotLog { get; set; }

        WarehouseTable PickPort { get; set; }

        WarehouseTable DropPort { get; set; }
    }
}
