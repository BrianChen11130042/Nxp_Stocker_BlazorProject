using CommonLibraryB_NXP.Manager.ModbusTcp.Master;
using CommonLibraryB_NXP.Tools.LogWritter;

namespace NXP_Stocker_BlazorProject.Scope
{

    public partial class MachineScope
    {
        public ModbusTcpMasterManager modbusTcpMasterManager;

        void createManager()
        {
            modbusTcpMasterManager = provider.GetRequiredService<ModbusTcpMasterManager>();
        }

        void initManager()
        {
            string logTcp = string.Empty;

            if(modbusTcpMasterManager.Connect(out logTcp))
            {
                mainDataService.IsModbusConnect = true;
            }
            else
            {
                mainDataService.IsModbusConnect = false;
                observerService.NotifyNLog(EStatus.Error, logTcp);
            }
        }
    }
}
