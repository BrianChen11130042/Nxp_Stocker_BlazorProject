namespace NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface
{

    public interface IMissionAsignTaskPack
    {
        Task<bool> GetPlcPierName();

        Task<bool> GetTableNewMissionAsign();

        Task UpdateUIMissionAsign();

        bool IsInputWarehouse();

        bool IsOutputWarehouse();

        bool IsTransformWarehouse();
    }
}
