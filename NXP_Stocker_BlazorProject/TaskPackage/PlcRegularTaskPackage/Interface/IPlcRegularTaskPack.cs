namespace NXP_Stocker_BlazorProject.TaskPackage.PlcRegularTaskPackage.Interface
{

    public interface IPlcRegularTaskPack
    {
        Task<bool> SetPlcHeartBeat();

        Task<bool> GetPlcWarehouse();

        Task UpdateUIWarehouse();
    }
}
