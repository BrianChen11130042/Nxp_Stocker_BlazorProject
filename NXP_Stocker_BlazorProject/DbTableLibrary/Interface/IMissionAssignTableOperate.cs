namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IMissionAssignTableOperate
    {
        Task<(bool status, string msg, MissionAsignTable table)> AddMainMission(MissionAsignTable data);

        Task<(bool status, string msg, MissionAsignTable table)> GetNewMainMission(string PierName);
    }
}
