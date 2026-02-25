using CommonLibraryB_NXP.Library.PLC.Config;
using CommonLibraryB_NXP.Library.UPS.Config;
using CommonLibraryB_NXP.Manager.ModbusRtu;
using CommonLibraryB_NXP.Manager.ModbusTcp.Master;
using NXP_Stocker_BlazorProject.DeviceName.PLC;
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


        Task<List<ModbusRtuConfig>> GetModbusRtuConfig();

        Task SetModbusRtuConfig(ModbusRtuConfig config);

        Task<List<string>> GetModbusRtuComList();

        Task<List<UpsConfig>> GetUpsConfig();

        Task SetUpsConfig(UpsConfig config);


        Task Initial();

        event dgInitMessage dgInitMsg;

        //任務
        Task<List<MissionAssignTable>> GetMissionAsignFromInQue();

        Task<bool> SetMission(MissionInform mission);

        Task<bool> DeleteMission(Guid id);

        event dgMissionAssignAction dgMissionAssignAction;

        //倉儲
        event dgWarehouseInform dgWhInform;

        // 要砍掉!!!
        event dgPlcActionStatus dgPlcAction; //要砍掉!!!
        Task<Dictionary<EPLC, bool>> GetDcPlcAction(); //要砍掉!!!

        //Machine Unit Status
        event dgMachineUnitStatus dgMachineUnitStatus;
        Task<Dictionary<EMachineUnit, MachineUnitStatus>> GetMachineUnitStatus();


        //UPS
        event dgUpsStatusInform dgUpsInform;

    }
}
