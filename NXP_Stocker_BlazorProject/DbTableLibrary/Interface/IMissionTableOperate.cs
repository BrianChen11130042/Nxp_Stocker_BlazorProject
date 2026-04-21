using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IMissionTableOperate
    {
        Task<(bool status, string msg)> InitMissionAsignToInQue();

        Task<List<MissionAssignTable>> GetMissionAssignFromInQueue();

        Task<(bool status, string msg, List<MissionAssignTable> list)> GetMissionAssignHistory(int pierNo);

        Task<(bool status, string msg, MissionAssignTable table)> GetNewMissionAsign(bool IsStart, bool IsFinish, 
                                                                                     bool IsCancel, int PierNo);

        Task<(bool status, string msg, MissionAssignTable table)> UpSertMissionAsign(MissionAssignTable data);

        Task<(bool status, string msg, T table)> GetNewMission<T>(bool IsStart, bool IsFinish, int PierNo = 0) where T : MissionBase;

        Task<(bool status, string msg, T table)> GetMissionById<T>(Guid Id) where T : MissionBase;

        Task<(bool status, string msg, T table)> UpSertMission<T>(T data) where T : MissionBase;

        event Func<Task>? MissionAssignInQueueChangedAct;

        Task UpdateMissionStatusToInQue<T>(T data) where T : MissionBase;

        Task RemoveFinishedMission();
    }
}
