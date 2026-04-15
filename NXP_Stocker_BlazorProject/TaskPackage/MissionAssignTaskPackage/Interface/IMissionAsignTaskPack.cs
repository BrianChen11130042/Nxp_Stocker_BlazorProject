namespace NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface
{

    public interface IMissionAsignTaskPack
    {
        //公用
        Task<bool> GetPlcPierNo();

        Task<bool> GetTableNewMissionAsign();

        Task UpdateUIMissionAsign();

        Task UpdateUIMissionAsignLog();

        Task UpdateUIPickPortWarehouse();

        Task UpdateUIDropPortWarehouse();

        Task<bool> GetTableWarehousePickPort();

        Task<bool> GetTableWarehouseDropPort();

        Task<bool> SetTableWarehouseInputPickPort();

        Task<bool> SetTableWarehouseOutputPickPort();

        Task<bool> SetTableWarehouseInputDropPort();

        Task<bool> SetTableWarehouseOutputDropPort();

        Task<bool> SetTableNewPierMission();

        Task<bool> SetTableNewPierOutputMission();

        Task<bool> GetTablePierMissionStatus();

        bool IsPierMissionFinish();

        bool IsPierMissionError();

        Task<bool> SetTableMissionAsignStart();

        Task<bool> SetTableMissionAsignError();

        Task<bool> SetLogMissionAsignStart();

        Task<bool> SetLogMissionAsignError();

        Task<bool> SetTableMissionAsignFinsih();

        Task<bool> SetLogMissionAsignFinish();

        Task<bool> SetTableNewRobotMission();

        Task<bool> GetTableRobotMissionStatus();

        bool IsRobotMissionBarcodeFail();

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
