using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{

    public interface IMainDataService
    {
        List<LogTable> ListMainLog { get; set; }

        bool IsModbusConnect { get; set; }

        //MissionAsignTable
        Task<bool> InitMissionAsignByInQue();

        //LogTable
        Task<bool> AddLogByMainTask(string type, string log);
    }
}
