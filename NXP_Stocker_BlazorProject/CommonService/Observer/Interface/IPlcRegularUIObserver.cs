using NXP_Stocker_BlazorProject.CommonService.Data;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IPlcRegularUIObserverable
    {
        void AddPlcRegularUIObserver(IPlcRegularUIObserver o);

        void RemovePlcRegularUIObserver(IPlcRegularUIObserver o);

        Task NotifyWarehouseInform(Dictionary<int, EWhStatus> dcWh);
    }

    public interface IPlcRegularUIObserver
    {
        Task UpdateWarehouseInform(Dictionary<int, EWhStatus> dcWh);
    }
}
