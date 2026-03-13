using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;
using NXP_Stocker_BlazorProject.MachineModel;

namespace NXP_Stocker_BlazorProject.CommonService.Data.Interface
{
    public interface IMissionAsignDataService
    {
        int PierNo { get; set; }

        int ErrorCode { get; set; }

        MissionAssignTable MissionAsign { get; set; }

        WarehouseInform PickPort { get; set; }

        WarehouseInform DropPort { get; set; }

        PierMissionTable PierMission { get; set; }

        RobotMissionTable RobotMission { get; set; }

        List<LogTable> ListMissionAsignLog { get; set; }

        //MissionAsignTable
        Task<bool> GetNewMissionAsignTable();

        Task<bool> SetMissionAsignTable();

        //PierMissionTable
        Task<bool> SetNewPierMissionTable();

        Task<bool> GetTargetPierMissionTable();

        //RobotMissionTable
        Task<bool> SetNewRobotMissionTable();

        Task<bool> GetTargetRobotMissionTable();

        //LogTable
        Task<bool> AddLogByMissionAsign(string type, string log);
    }
}
