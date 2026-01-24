namespace NXP_Stocker_BlazorProject.TaskPackage.ThreadTaskPackage.Interface
{
    public interface IThreadTaskPack
    {
        Task<bool> InitMissionAsignInQue();

        Task<bool> CheckPlcConnect();

        Task<bool> SetLogInitSuccess();

        Task<bool> SetLogInitFail();

        Task UpdateUIPopInitSuccess();

        Task UpdateUIPopInitFail();

        Task<bool> SetLogConnectFail();

        Task UpdateUIPopConnectFail();

        Task UpdateUIMainLog();
    }
}
