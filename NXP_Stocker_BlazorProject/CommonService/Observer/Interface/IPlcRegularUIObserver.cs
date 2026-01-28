using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.MachineModel;

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
