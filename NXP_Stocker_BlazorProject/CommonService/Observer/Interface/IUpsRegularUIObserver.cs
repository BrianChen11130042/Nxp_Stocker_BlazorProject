using NXP_Stocker_BlazorProject.MachineModel;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IUpsRegularUIObserverable
    {
        void AddUpsRegularUIObserver(IUpsRegularUIObserver o);

        void RemoveUpsRegularUIObserver(IUpsRegularUIObserver o);

        Task NotifyUpsStatusInform(UpsInform inform);

        Task NotifyUpsAction(int ups, int status);
    }

    public interface IUpsRegularUIObserver
    {
        Task UpdateUpsStatusInform(UpsInform inform);

        Task UpdateUpsAction(int ups, int status);
    }
}
