using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{

    public interface IRobotDataService
    {
        int RobotNo { get; set; }

        RobotMissionTable RobotMission { get; set; }

        List<LogTable> ListRobotLog { get; set; }

        //RobotMissionTable
        Task<bool> GetNewRobotMissionTable();

        Task<bool> SetRobotMissionTable();

        Task UpdateRobotMissionStatusToInque();

        //LogTable
        Task<bool> AddLogByRobot(string type, string log);
    }
}
