namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{

    public interface IPierMissionTableOperate
    {
        Task<(bool status, string msg, PierMissionTable table)> AddPierMission(PierMissionTable data);
    }
}
