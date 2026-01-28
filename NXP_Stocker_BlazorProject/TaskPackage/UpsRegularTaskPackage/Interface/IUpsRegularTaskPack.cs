namespace NXP_Stocker_BlazorProject.TaskPackage.UpsRegularTaskPackage.Interface
{

    public interface IUpsRegularTaskPack
    {
        Task<bool> GetUpsStatus();

        Task UpdateUpsStatus();
    }
}
