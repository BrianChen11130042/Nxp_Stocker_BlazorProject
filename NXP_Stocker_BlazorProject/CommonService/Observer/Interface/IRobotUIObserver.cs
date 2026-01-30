using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IRobotUIObserverable
    {
        void AddRobotUIObserver(IRobotUIObserver o);

        void RemoveRobotUIObserver(IRobotUIObserver o);

        Task NotifyRobotMission(int robot, RobotMissionTable table);

        Task NotifyRobotLog(int robot, List<LogTable> list);

        Task NotifyRobotAction(int robot, int status);
    }

    public interface IRobotUIObserver
    {
        Task UpdateRobotMission(int robot, RobotMissionTable table);

        Task UpdateRobotLog(int robot, List<LogTable> list);

        Task UpdateRobotAction(int robot, int status);
    }
}
