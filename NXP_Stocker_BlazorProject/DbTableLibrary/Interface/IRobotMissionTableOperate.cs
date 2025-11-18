using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{

    public interface IRobotMissionTableOperate
    {
        Task<(bool status, string msg, RobotMissionTable_stub table)> AddRobotMission(RobotMissionTable_stub data);

        Task<(bool status, string msg, RobotMissionTable_stub table)> GetNewRobotMission();

        Task<(bool status, string msg, RobotMissionTable_stub table)> UpdateRobotMission(RobotMissionTable_stub data);

        Task<(bool status, string msg, RobotMissionTable_stub table)> GetTargetRobotMission(RobotMissionTable_stub data);
    }
}
