using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface ILogTableOperate
    {
        Task<(bool status, string msg, List<LogTable_stub> list)> AddLogData(LogTable_stub data);
    }
}
