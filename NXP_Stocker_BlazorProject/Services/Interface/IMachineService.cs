using CommonLibraryB_NXP.Library.PLC.Config;
using CommonLibraryB_NXP.Manager.ModbusTcp.Master;
using NXP_Stocker_BlazorProject.EFModel;
using NXP_Stocker_BlazorProject.MachineModel;

namespace NXP_Stocker_BlazorProject.Services.Interface
{
    public interface IMachineService
    {
        //連線初始化
        Task<List<ModbusTcpMasterConfig>> GetModbusTcpConfig();

        Task SetModbusTcpConfig(ModbusTcpMasterConfig config);

        Task<List<PlcConfig>> GetPlcConfig();

        Task SetPlcConfig(PlcConfig config);

        Task Initial();

        event dgInitMessage dgInitMsg;

        //任務
        Task<List<MissionAsignTable>> GetMissionAsignFromInQue();

        Task<bool> SetMission(MissionInform mission);
    }
}
