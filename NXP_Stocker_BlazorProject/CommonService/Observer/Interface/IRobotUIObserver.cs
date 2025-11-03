using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IRobotUIObserverable
    {
        void AddRobotUIObserver(IRobotUIObserver o);

        void RemoveRobotUIObserver(IRobotUIObserver o);

        Task NotifyRobotMission(string pier, RobotMissionTable table);

        Task NotifyRobotLog(string pier, List<LogTable> list);

        Task NotifyStorgePortTable(string pier, WarehouseTable pickPort, WarehouseTable dropPort);
    }

    public interface IRobotUIObserver
    {
        Task UpdateRobotMission(string pier, RobotMissionTable table);

        Task UpdateRobotLog(string pier, List<LogTable> list);

        Task UpdateStoragePortTable(string pier, WarehouseTable pickPort, WarehouseTable dropPort);
    }
}
