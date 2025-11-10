using CommonLibraryB_NXP.Manager.ModbusTcp.Master;

namespace NXP_Stocker_BlazorProject.Services.Interface
{

    public interface IMachineService
    {
        //Config
        Task<List<ModbusTcpMasterConfig>> GetModbusTcpConfig();
    }
}
