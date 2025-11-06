namespace NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface
{

    public interface IMissionAsignTaskPack
    {
        Task<bool> GetPlcPierName();

        Task<bool> GetTableNewMissionAsign();

        Task UpdateUIMissionAsign();

        Task<bool> GetTableWarehousePickPort();

        Task<bool> GetTableWarehouseDropPort();

        Task<bool> SetTableNewPierMission();

        Task<bool> SetTableMissionAsignStart();

        //入庫
        bool IsInputWarehouse();

        //出庫
        bool IsOutputWarehouse();

        //庫位轉移
        bool IsTransformWarehouse();
    }
}
