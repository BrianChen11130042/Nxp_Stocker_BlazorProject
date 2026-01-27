using CommonLibraryB_NXP.Manager.ModbusRtu;
using CommonLibraryB_NXP.Manager.ModbusTcp.Master;
using CommonLibraryB_NXP.Tools.LogWritter;

namespace NXP_Stocker_BlazorProject.Scope
{

    public partial class MachineScope
    {
        public ModbusTcpMasterManager modbusTcpMasterManager;
        public ModbusRtuManager modbusRtuManager;

        void createManager()
        {
            modbusTcpMasterManager = provider.GetRequiredService<ModbusTcpMasterManager>();
            modbusRtuManager = provider.GetRequiredService<ModbusRtuManager>();
        }

        void initManager()
        {
            bool tcp = false;
            bool rtu = false;

            string logTcp = string.Empty;
            string logRtu = string.Empty;

            if (modbusTcpMasterManager.Connect(out logTcp))
            {
                tcp = true;
            }
            else
            {
                observerService.NotifyNLog(EStatus.Error, logTcp);
            }

            if(modbusRtuManager.Connect(out logRtu))
            {
                rtu = true;
            }
            else
            {
                observerService.NotifyNLog(EStatus.Error, logRtu);
            }

            if(tcp == rtu == true)
            {
                mainDataService.IsModbusConnect = true;
            }
            else
            {
                mainDataService.IsModbusConnect = false;
            }
        }
    }
}
