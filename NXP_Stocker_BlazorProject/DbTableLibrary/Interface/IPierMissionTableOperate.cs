using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{

    public interface IPierMissionTableOperate
    {
        Task<(bool status, string msg, PierMissionTable_stub table)> AddPierMission(PierMissionTable_stub data);

        Task<(bool status, string msg, PierMissionTable_stub table)> GetNewPierMission(string PierName);

        Task<(bool status, string msg, PierMissionTable_stub table)> UpdatePierMission(PierMissionTable_stub data);

        Task<(bool status, string msg, PierMissionTable_stub table)> GetTargetPierMission(PierMissionTable_stub data);
    }
}
