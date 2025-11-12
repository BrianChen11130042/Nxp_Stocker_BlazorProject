using CommonLibraryB_NXP.Library.PLC.Config;
using CommonLibraryB_NXP.Manager.ModbusTcp.Master;
using NXP_Stocker_BlazorProject.DeviceName.PLC;
using NXP_Stocker_BlazorProject.Scope;
using NXP_Stocker_BlazorProject.Services.Interface;

namespace NXP_Stocker_BlazorProject.Services
{
    public partial class MachineService
    {
        public MachineScope scope;

        public MachineService(MachineScope scope)
        {
            this.scope = scope;
        }
    }

    public partial class MachineService : IMachineService
    {
        public async Task<List<ModbusTcpMasterConfig>> GetModbusTcpConfig()
        {
            List<ModbusTcpMasterConfig> list = new List<ModbusTcpMasterConfig>();

            foreach(string dev in Enum.GetNames(typeof(EModbusTcpMaster)))
            {
                ModbusTcpMasterConfig config = scope.modbusTcpMasterManager.Get(dev);

                if(config != null)
                {
                    list.Add(config);
                }
            }

            return list;
        }

        public async Task SetModbusTcpConfig(ModbusTcpMasterConfig config)
        {
            scope.modbusTcpMasterManager.Set(config.device, config);
            scope.modbusTcpMasterManager.Save();
        }

        public async Task<List<PlcConfig>> GetPlcConfig()
        {
            List<PlcConfig> list = new List<PlcConfig>();

            foreach(string dev in Enum.GetNames(typeof(EPLC)))
            {
                PlcConfig config = scope.plcConfig.Get(dev);

                if(config != null)
                {
                    list.Add(config);
                }
            }

            return list;
        }

        public async Task SetPlcConfig(PlcConfig config)
        {
            scope.plcConfig.Set(config.device, config);
            scope.plcConfig.Save();
        }
    }
}
