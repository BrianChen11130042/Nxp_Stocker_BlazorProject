using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IRobotUIObserverable
    {
        void AddRobotUIObserver(IRobotUIObserver o);

        void RemoveRobotUIObserver(IRobotUIObserver o);

        Task NotifyRobotMission(int pier, RobotMissionTable table);

        Task NotifyRobotLog(int pier, List<LogTable> list);
    }

    public interface IRobotUIObserver
    {
        Task UpdateRobotMission(int pier, RobotMissionTable table);

        Task UpdateRobotLog(int pier, List<LogTable> list);
    }
}
