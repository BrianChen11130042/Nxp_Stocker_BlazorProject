using CommonLibraryB_NXP.Library.PLC.Config;
using CommonLibraryB_NXP.Manager.ModbusTcp.Master;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DeviceName.PLC;
using NXP_Stocker_BlazorProject.Scope;
using NXP_Stocker_BlazorProject.Services.Interface;

namespace NXP_Stocker_BlazorProject.Services
{
    public partial class MachineService : IMachineService
    {
        public MachineScope scope;

        public MachineService(MachineScope scope)
        {
            this.scope = scope;

            scope.observerService.AddMainUIObserver(this);
        }
    }

    public delegate Task dgInitMessage(bool popUp, string msg);

    public partial class MachineService : IMainUIObserver
    {
        public event dgInitMessage dgInitMsg;

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

        public async Task Initial()
        {
            scope.initAll();
        }

        public async Task UpdateMainLog(List<LogTable_stub> list)
        {

        }

        public async Task UpdatePopUpMessage(bool popUp, string msg)
        {
            dgInitMsg?.Invoke(popUp, msg);
        }
    }

    public partial class MachineService
    {
        public async Task<bool> SetMission(EPier pier, EMission mission, string barcode, EBoardSize size = EBoardSize.Large)
        {
            switch(mission)
            {
                case EMission.InputWarehouse:
                    return await SetInputWarehouseMission(pier.ToString(), (int)mission, barcode, (int)size);

                case EMission.OutputWarehouse:
                    return await SetOutputWarehouseMission(pier.ToString(), (int)mission, barcode);

                default:
                    return false;
            }
        }

        async Task<bool> SetInputWarehouseMission(string pier, int mission, string barcode, int size)
        {
            var pickResult = await scope.IWarehouseTableOp.GetEmptyPier(pier, size);

            if (pickResult.status == false)
            {
                return false;
            }

            var dropResult = await scope.IWarehouseTableOp.GetEmptyStorage(pier, size);

            if (dropResult.status == false)
            {
                return false;
            }

            MissionAsignTable_stub missionAsignTable = new MissionAsignTable_stub()
            {
                PierName = pier,
                MissionSerialNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                ActionCode = mission,
                Barcode = barcode,
                BoardSize = size,
                PickZone = pickResult.table.Zone,
                PickLayer = pickResult.table.Layer,
                DropZone = dropResult.table.Zone,
                DropLayer = dropResult.table.Layer,
                EstablishTime = DateTime.Now
            };

            var result = await scope.IMissionAsignTableOp.AddMissionAsign(missionAsignTable);

            if(result.status == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        async Task<bool> SetOutputWarehouseMission(string pier, int mission, string barcode)
        {
            var pickResult = await scope.IWarehouseTableOp.GetPickTarget(pier, barcode);

            if (pickResult.status == false)
            {
                return false;
            }

            var dropResult = await scope.IWarehouseTableOp.GetEmptyPier(pier, pickResult.table.BoardSize);

            if (dropResult.status == false)
            {
                return false;
            }

            MissionAsignTable_stub missionAsignTable = new MissionAsignTable_stub()
            {
                PierName = pier,
                MissionSerialNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                ActionCode = mission,
                Barcode = barcode,
                BoardSize = pickResult.table.BoardSize,
                PickZone = pickResult.table.Zone,
                PickLayer = pickResult.table.Layer,
                DropZone = dropResult.table.Zone,
                DropLayer = dropResult.table.Layer,
                EstablishTime = DateTime.Now
            };

            var result = await scope.IMissionAsignTableOp.AddMissionAsign(missionAsignTable);

            if (result.status == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
