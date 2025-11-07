namespace NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface
{

    public interface IMissionAsignTaskPack
    {
        //公用
        Task<bool> GetPlcPierName();

        Task<bool> GetTableNewMissionAsign();

        Task UpdateUIMissionAsign();

        Task UpdateUIMissionAsignLog();

        Task<bool> GetTableWarehousePickPort();

        Task<bool> GetTableWarehouseDropPort();

        Task<bool> SetTableWarehouseInputPickPort();

        Task<bool> SetTableWarehouseOutputPickPort();

        Task<bool> SetTableWarehouseInputDropPort();

        Task<bool> SetTableWarehouseOutputDropPort();

        Task<bool> SetTableNewPierMission();

        Task<bool> GetTablePierMissionStatus();

        bool IsPierMissionFinish();

        Task<bool> SetTableMissionAsignStart();

        Task<bool> SetLogMissionAsignStart();

        Task<bool> SetTableMissionAsignFinsih();

        Task<bool> SetLogMissionAsignFinish();

        Task<bool> SetTableNewRobotMission();

        Task<bool> GetTableRobotMissionStatus();

        bool IsRobotMissionError();

        bool IsRobotMissionFinish();

        //入庫
        bool IsInputWarehouse();

        //出庫
        bool IsOutputWarehouse();

        //庫位轉移
        bool IsTransformWarehouse();
    }
}
