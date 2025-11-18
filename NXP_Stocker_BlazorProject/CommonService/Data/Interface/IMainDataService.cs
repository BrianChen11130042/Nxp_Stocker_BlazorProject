using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{

    public interface IMainDataService
    {
        List<LogTable_stub> ListMainLog { get; set; }

        bool IsModbusConnect { get; set; }

        //LogTable
        Task<bool> AddLogByMainTask(string type, string log);
    }
}
