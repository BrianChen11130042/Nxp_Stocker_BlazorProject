using CommonLibraryB_NXP.Library.PLC.Config;
using CommonLibraryB_NXP.Manager.ModbusTcp.Master;

namespace NXP_Stocker_BlazorProject.Services.Interface
{
    public enum EPier
    {
        Pier1,
        Pier2
    }

    public enum EMission
    {
        InputWarehouse = 1,
        OutputWarehouse = 2,
        TransToBuffer = 3
    }

    public enum EBoardSize
    {
        Large = 0,
        Small = 1
    }

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
        Task<bool> SetMission(EPier pier, EMission mission, string barcode, EBoardSize size);
    }
}
