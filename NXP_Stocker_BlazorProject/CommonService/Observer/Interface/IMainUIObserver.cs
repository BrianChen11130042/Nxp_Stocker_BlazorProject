using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IMainUIObserverable
    {
        void AddMainUIObserver(IMainUIObserver o);

        void RemoveMainUIObserver(IMainUIObserver o);

        Task NotifyPopUpMessage(bool popUp, string msg);

        Task NotifyMainLog(List<LogTable> list);

        Task NotifyWarehouseInform(Dictionary<int, EWhStatus> dcWh);
    }

    public interface IMainUIObserver
    {
        Task UpdatePopUpMessage(bool popUp, string msg);

        Task UpdateMainLog(List<LogTable> list);

        Task UpdateWarehouseInform(Dictionary<int, EWhStatus> dcWh);
    }
}
