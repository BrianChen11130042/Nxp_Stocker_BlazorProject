namespace NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage.Interface
{

    public interface IRobotTaskPack
    {
        Task<bool> GetRobotStatus();

        bool IsRobotError();

        Task<bool> GetTableNewMission();

        Task UpdateUIRobotMission();

        bool IsGetNewMission();
    }
}
