namespace NXP_Stocker_BlazorProject.TaskPackage.MainTaskPackage.Interface
{
    public interface IMainTaskPack
    {
        Task<bool> CheckDbConnect();

        Task<bool> CheckPlcConnect();

        Task<bool> SetLogInitSuccess();

        Task<bool> SetLogInitFail();

        Task UpdateUIInitSuccess();

        Task UpdateUIInitFail();
    }
}
