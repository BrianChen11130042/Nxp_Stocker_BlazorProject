using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{

    public interface IPierMissionTableOperate
    {
        Task<(bool status, string msg, PierMissionTable table)> AddPierMission(PierMissionTable data);

        Task<(bool status, string msg, PierMissionTable table)> GetNewPierMission(int PierNo);

        Task<(bool status, string msg, PierMissionTable table)> UpdatePierMission(PierMissionTable data);

        Task<(bool status, string msg, PierMissionTable table)> GetTargetPierMission(PierMissionTable data);
    }
}
