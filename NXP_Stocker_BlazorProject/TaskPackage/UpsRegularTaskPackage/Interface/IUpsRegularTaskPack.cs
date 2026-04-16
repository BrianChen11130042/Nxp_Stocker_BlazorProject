namespace NXP_Stocker_BlazorProject.TaskPackage.UpsRegularTaskPackage.Interface
{

    public interface IUpsRegularTaskPack
    {
        Task<bool> GetUpsNo();

        Task<bool> GetUpsStatus();

        Task UpdateUpsStatus();

        Task UpdateUIUpsRunning();

        Task UpdateUIUpsDisconnect();
    }
}
