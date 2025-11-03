using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IPierUIObserverable
    {
        void AddPierUIObserver(IPierUIObserver o);

        void RemovePierUIObserver(IPierUIObserver o);

        Task NotifyPierMission(string pier, PierMissionTable table);

        Task NotifyPierLog(string pier, List<LogTable> list);

        Task NotifyPierTable(string pier, WarehouseTable pierTable);
    }


    public interface IPierUIObserver
    {
        Task UpdatePierMission(string pier, PierMissionTable table);

        Task UpdatePierLog(string pier, List<LogTable> list);

        Task UpdatePierTable(string pier, WarehouseTable pierTable);
    }
}
