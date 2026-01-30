using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IPierUIObserverable
    {
        void AddPierUIObserver(IPierUIObserver o);

        void RemovePierUIObserver(IPierUIObserver o);

        Task NotifyPierMission(int pier, PierMissionTable table);

        Task NotifyPierLog(int pier, List<LogTable> list);

        Task NotifyPierAction(int pier, int status);
    }


    public interface IPierUIObserver
    {
        Task UpdatePierMission(int pier, PierMissionTable table);

        Task UpdatePierLog(int pier, List<LogTable> list);

        Task UpdatePierAction(int pier, int status);
    }
}
