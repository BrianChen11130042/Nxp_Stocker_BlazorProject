using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IPierUIObserverable
    {
        void AddPierUIObserver(IPierUIObserver o);

        void RemovePierUIObserver(IPierUIObserver o);

        Task NotifyPierMission(string pier, PierMissionTable_stub table);

        Task NotifyPierLog(string pier, List<LogTable_stub> list);
    }


    public interface IPierUIObserver
    {
        Task UpdatePierMission(string pier, PierMissionTable_stub table);

        Task UpdatePierLog(string pier, List<LogTable_stub> list);
    }
}
