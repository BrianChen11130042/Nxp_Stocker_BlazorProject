namespace NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage.Interface
{

    public interface IRobotTaskPack
    {
        Task<bool> GetRobotStatus();

        bool IsRobotError();

        Task<bool> GetTableNewMission();

        Task UpdateUIRobotMission();

        Task UpdateRobotMissionStatusToInQue();

        Task UpdateUIRobotLog();

        bool IsGetNewMission();

        Task<bool> SetPlcRobotMission();

        Task<bool> SetPlcRobotStart();

        Task<bool> SetTableMissionStart();

        Task<bool> SetLogMissionStart();

        Task<bool> GetPlcRobotStatus();

        bool IsRobotFinish();

        Task<bool> SetPlcRobotFinish();

        Task<bool> SetTableMissionFinish();

        Task<bool> SetLogMissionFinish();
    }
}
