using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IRobotUIObserverable
    {
        void AddRobotUIObserver(IRobotUIObserver o);

        void RemoveRobotUIObserver(IRobotUIObserver o);

        Task NotifyRobotMission(string pier, RobotMissionTable_stub table);

        Task NotifyRobotLog(string pier, List<LogTable_stub> list);
    }

    public interface IRobotUIObserver
    {
        Task UpdateRobotMission(string pier, RobotMissionTable_stub table);

        Task UpdateRobotLog(string pier, List<LogTable_stub> list);
    }
}
