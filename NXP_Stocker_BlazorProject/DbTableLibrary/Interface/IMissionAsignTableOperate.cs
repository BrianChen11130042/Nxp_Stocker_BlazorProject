using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IMissionAsignTableOperate
    {
        Task<(bool status, string msg, MissionAsignTable table)> AddMissionAsign(MissionAsignTable data);

        Task<(bool status, string msg, MissionAsignTable table)> GetNewMissionAsign(int PierNo);

        Task<(bool status, string msg, MissionAsignTable table)> UpdateMissionAsign(MissionAsignTable data);
    }
}
