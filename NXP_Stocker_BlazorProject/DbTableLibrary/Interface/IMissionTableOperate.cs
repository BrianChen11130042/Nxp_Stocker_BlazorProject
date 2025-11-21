using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IMissionTableOperate
    {
        Task<(bool status, string msg)> InitMissionAsignToInQue();

        Task<List<MissionAsignTable>> GetMissionAsignFromInQue();

        Task<(bool status, string msg, MissionAsignTable table)> GetNewMissionAsign(bool IsStart, bool IsFinish, int PierNo);

        Task<(bool status, string msg, MissionAsignTable table)> UpSertMissionAsign(MissionAsignTable data);

        Task<(bool status, string msg, T table)> GetNewMission<T>(bool IsStart, bool IsFinish, int PierNo = 0) where T : MissionBase;

        Task<(bool status, string msg, T table)> GetMissionById<T>(Guid Id) where T : MissionBase;

        Task<(bool status, string msg, T table)> UpSertMission<T>(T data) where T : MissionBase;

        Task UpdateMissionStatusToInQue<T>(T data) where T : MissionBase;
    }
}
