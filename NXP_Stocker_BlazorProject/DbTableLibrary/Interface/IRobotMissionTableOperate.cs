namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{

    public interface IRobotMissionTableOperate
    {
        Task<(bool status, string msg, RobotMissionTable table)> AddRobotMission(RobotMissionTable data);
    }
}
