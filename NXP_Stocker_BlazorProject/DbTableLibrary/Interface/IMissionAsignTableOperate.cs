using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IMissionAsignTableOperate
    {
        Task<(bool status, string msg, MissionAsignTable_stub table)> AddMissionAsign(MissionAsignTable_stub data);

        Task<(bool status, string msg, MissionAsignTable_stub table)> GetNewMissionAsign(string PierName);

        Task<(bool status, string msg, MissionAsignTable_stub table)> UpdateMissionAsign(MissionAsignTable_stub data);
    }
}
