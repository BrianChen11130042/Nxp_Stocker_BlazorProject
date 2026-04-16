namespace NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage.Interface
{

    public interface IRobotTaskPack
    {
        Task<bool> GetPlcRobotNo();

        Task<bool> GetRobotIsReady();

        bool IsRobotReady();

        Task<bool> GetTableNewMission();

        Task UpdateUIRobotMission();

        Task UpdateRobotMissionStatusToInQue();

        Task UpdateUIRobotLog();

        Task UpdateUIRobotDisconnect();

        Task UpdateUIRobotStop();

        Task UpdateUIRobotIdle();

        Task UpdateUIRobotRunning();

        Task UpdateUIRobotMotionStatus();

        bool IsGetNewMission();

        Task<bool> SetPlcRobotMission();

        Task<bool> SetPlcRobotStart();

        Task<bool> SetTableMissionStart();

        Task<bool> SetLogMissionStart();

        Task<bool> GetPlcRobotStatus();

        Task<bool> GetPlcIsRobotError();

        bool IsRobotFinish();

        bool IsRobotError();

        bool IsBarcodeScanError();

        bool IsNotBarcodeScanError();

        Task<bool> SetPlcRobotFinish();

        Task<bool> SetTableMissionFinish();

        Task<bool> SetTableMissionError();

        Task<bool> SetPlcRobotRevert();

        Task<bool> SetLogMissionFinish();

        Task<bool> SetLogMissionError();
    }
}
