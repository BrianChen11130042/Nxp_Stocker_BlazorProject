namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{

    public interface IRobotMissionTableOperate
    {
        Task<(bool status, string msg, RobotMissionTable table)> AddRobotMission(RobotMissionTable data);

        Task<(bool status, string msg, RobotMissionTable table)> GetNewRobotMission();

        Task<(bool status, string msg, RobotMissionTable table)> UpdateRobotMission(RobotMissionTable data);

        Task<(bool status, string msg, RobotMissionTable table)> GetTargetRobotMission(RobotMissionTable data);
    }
}
