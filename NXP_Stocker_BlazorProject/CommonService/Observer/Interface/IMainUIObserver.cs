using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IMainUIObserverable
    {
        void AddMainUIObserver(IMainUIObserver o);

        void RemoveMainUIObserver(IMainUIObserver o);

        Task NotifyPopUpMessage(bool popUp, string msg);

        Task NotifyMainLog(List<LogTable> list);
    }

    public interface IMainUIObserver
    {
        Task UpdatePopUpMessage(bool popUp, string msg);

        Task UpdateMainLog(List<LogTable> list);
    }
}
