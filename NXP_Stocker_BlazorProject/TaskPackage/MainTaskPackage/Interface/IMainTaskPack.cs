namespace NXP_Stocker_BlazorProject.TaskPackage.MainTaskPackage.Interface
{
    public interface IMainTaskPack
    {
        Task<bool> CheckDbConnect();

        Task<bool> CheckPlcConnect();

        Task<bool> SetLogInitSuccess();

        Task<bool> SetLogInitFail();

        Task UpdateUIPopInitSuccess();

        Task UpdateUIPopInitFail();

        Task<bool> SetPlcHeartBeat();

        Task<bool> SetLogConnectFail();

        Task UpdateUIPopConnectFail();

        Task UpdateUIMainLog();
    }
}
