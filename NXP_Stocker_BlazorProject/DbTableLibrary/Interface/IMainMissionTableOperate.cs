namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IMainMissionTableOperate
    {
        Task<(bool status, string msg, MainMissionTable table)> AddMainMissionTable(MainMissionTable data);
    }
}
