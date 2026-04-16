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
            observerService.NotifyInitUnitStatus(1, 901);
            observerService.NotifyInitUnitStatus(2, 901);
            observerService.NotifyInitUnitStatus(3, 901);
            observerService.NotifyInitUnitStatus(4, 901);

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

                observerService.NotifyInitUnitStatus(1, 903);
                observerService.NotifyInitUnitStatus(2, 903);
                observerService.NotifyInitUnitStatus(3, 903);
                observerService.NotifyInitUnitStatus(4, 903);
            }
            else
            {
                mainDataService.IsModbusConnect = false;
            }
        }

        void reconnectModbusTcpManager()
        {
            observerService.NotifyInitUnitStatus(1, 901);
            observerService.NotifyInitUnitStatus(2, 901);
            observerService.NotifyInitUnitStatus(3, 901);

            string logTcp = string.Empty;

            if (modbusTcpMasterManager.Connect(out logTcp))
            {
                observerService.NotifyInitUnitStatus(1, 903);
                observerService.NotifyInitUnitStatus(2, 903);
                observerService.NotifyInitUnitStatus(3, 903);
            }
            else
            {
                observerService.NotifyNLog(EStatus.Error, logTcp);
            }
        }

        void reconnectModbusRtuManager()
        {
            observerService.NotifyInitUnitStatus(4, 901);

            string logRtu = string.Empty;

            if (modbusRtuManager.Connect(out logRtu))
            {
                observerService.NotifyInitUnitStatus(4, 903);
            }
            else
            {
                observerService.NotifyNLog(EStatus.Error, logRtu);
            }
        }
    }
}
