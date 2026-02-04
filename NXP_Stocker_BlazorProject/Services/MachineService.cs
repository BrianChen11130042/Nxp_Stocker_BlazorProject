using CommonLibraryB_NXP.Library.PLC.Config;
using CommonLibraryB_NXP.Manager.ModbusTcp.Master;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DeviceName.PLC;
using NXP_Stocker_BlazorProject.Scope;
using NXP_Stocker_BlazorProject.Services.Interface;
using NXP_Stocker_BlazorProject.MachineModel;
using NXP_Stocker_BlazorProject.EFModel;
using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.CommonService.Data;
using CommonLibraryB_NXP.Manager.ModbusRtu;
using CommonLibraryB_NXP.Library.UPS.Config;
using NXP_Stocker_BlazorProject.DeviceName.UPS;

namespace NXP_Stocker_BlazorProject.Services
{
    public partial class MachineService : IMachineService
    {
        public MachineScope scope;

        public MachineService(MachineScope scope)
        {
            this.scope = scope;

            scope.observerService.AddMainUIObserver(this);
            scope.observerService.AddPierUIObserver(this);
            scope.observerService.AddRobotUIObserver(this);
            scope.observerService.AddPlcRegularUIObserver(this);
            scope.observerService.AddUpsRegularUIObserver(this);
            scope.observerService.AddMissionAssignUIObserver(this);
        }
    }

    public partial class MachineService
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


        public async Task<List<ModbusRtuConfig>> GetModbusRtuConfig()
        {
            List<ModbusRtuConfig> list = new List<ModbusRtuConfig>();

            foreach(var pair in scope.modbusRtuManager.table)
            {
                ModbusRtuConfig config = scope.modbusRtuManager.Get(pair.Key);

                if(config != null)
                {
                    list.Add(config);
                }
            }

            return list;
        }

        public async Task SetModbusRtuConfig(ModbusRtuConfig config)
        {
            scope.modbusRtuManager.Set(config.com, config);
            scope.modbusRtuManager.Save();
        }

        public async Task<List<string>> GetModbusRtuComList()
        {
            List<string> list = scope.modbusRtuManager.keys.ToList();

            return list;
        }

        public async Task<List<UpsConfig>> GetUpsConfig()
        {
            List<UpsConfig> list = new List<UpsConfig>();

            foreach(string dev in Enum.GetNames(typeof(EUPS)))
            {
                UpsConfig config = scope.upsConfig.Get(dev);

                if(config != null)
                {
                    list.Add(config);
                }
            }

            return list;
        }

        public async Task SetUpsConfig(UpsConfig config)
        {
            scope.upsConfig.Set(config.device, config);
            scope.upsConfig.Save();
        }

        public async Task Initial()
        {
            scope.initAll();
        }
    }

    public partial class MachineService
    {
        public async Task<List<MissionAssignTable>> GetMissionAsignFromInQue()
        {
            return await scope.IMissionTableOp.GetMissionAssignFromInQueue();
        }

        public async Task<bool> SetMission(MissionInform mission)
        {
            MissionAssignTable missionAsignTable = new MissionAssignTable()
            {
                Id = new Guid(),

                PierNo = mission.pierNo,
                ActionCode = mission.action,
                Barcode = mission.barcode,
                BoardSize = mission.size,
                PickZone = mission.pickZone,
                PickLayer = mission.pickLayer,
                DropZone = mission.dropZone,
                DropLayer = mission.dropLayer,
                EstablishTime = DateTime.Now
            };

            var result = await scope.IMissionTableOp.UpSertMissionAsign(missionAsignTable);

            if (result.status == true)
            {
                return true;
            }
            else
            {
                await scope.observerService.NotifyNLog(EStatus.Error, result.msg);
                return false;
            }
        }
    }

    public delegate Task dgInitMessage(bool popUp, string msg);

    public partial class MachineService : IMainUIObserver
    {
        public event dgInitMessage dgInitMsg;

        public async Task UpdatePopUpMessage(bool popUp, string msg)
        {
            dgInitMsg?.Invoke(popUp, msg);
        }

        public async Task UpdateInitUnitStatus(int deviceNo, int status)
        {
            await UpdateUnitStatus(deviceNo, status);
        }
    }

    public delegate Task dgMissionAssignAction(int pierNo, MissionAssignTable missionAsign);

    public partial class MachineService : IMissionAssignUIObserver
    {
        public event dgMissionAssignAction dgMissionAssignAction;

        public async Task UpdateMissionAssign(int pier, MissionAssignTable missionAsign)
        {
            dgMissionAssignAction?.Invoke(pier, missionAsign);
        }
    }

    public delegate Task dgPlcActionStatus(Dictionary<EPLC, bool> dcPlcAction); //要砍掉
    public delegate Task dgMachineUnitStatus(Dictionary<EMachineUnit, MachineUnitStatus> dcMachineUnitStatus); //取代上面的

    public partial class MachineService : IPierUIObserver
    {
        public event dgPlcActionStatus dgPlcAction; //要砍掉
        public event dgMachineUnitStatus dgMachineUnitStatus; //取代上面的

        Dictionary<EPLC, bool> dcPlcAction { get; set; } = new Dictionary<EPLC, bool>() //要砍掉
        {
            { EPLC.Pier1, false},
            { EPLC.Pier2, false},
            { EPLC.Robot, false}
        };
        Dictionary<EMachineUnit, MachineUnitStatus> dcMachineUnitStatus { get; set; } = new Dictionary<EMachineUnit, MachineUnitStatus>()//取代上面的
        {
            { EMachineUnit.Pier1, new MachineUnitStatus() },
            { EMachineUnit.Pier2, new MachineUnitStatus() },
            { EMachineUnit.Robot, new MachineUnitStatus() },
            { EMachineUnit.UPS, new MachineUnitStatus() }
        };

        public async Task<Dictionary<EPLC, bool>> GetDcPlcAction() //要砍掉
        {
            return dcPlcAction;
        }
        public async Task<Dictionary<EMachineUnit, MachineUnitStatus>> GetMachineUnitStatus()//取代上面的
        {
            return dcMachineUnitStatus;
        }

        public async Task UpdatePierAction(int pier, int status)
        {
            await UpdateUnitStatus(pier, status);
        }

    }

    public partial class MachineService : IRobotUIObserver
    {

        public async Task UpdateRobotAction(int robot, int status)
        {
            await UpdateUnitStatus(robot, status);
        }
    }

    public delegate Task dgWarehouseInform(Dictionary<int, EWhStatus> dcWh);

    public partial class MachineService : IPlcRegularUIObserver
    {
        public event dgWarehouseInform dgWhInform;

        public async Task UpdateWarehouseInform(Dictionary<int, EWhStatus> dcWh)
        {
            dgWhInform?.Invoke(dcWh);
        }
    }

    public delegate Task dgUpsStatusInform(UpsInform inform);

    public partial class MachineService : IUpsRegularUIObserver
    {
        public event dgUpsStatusInform dgUpsInform;

        public async Task UpdateUpsStatusInform(UpsInform inform)
        {
            dgUpsInform?.Invoke(inform);
        }

        public async Task UpdateUpsAction(int ups, int status)
        {
            await UpdateUnitStatus(ups, status);
        }
    }

    public partial class MachineService
    {
        async Task UpdateUnitStatus(int deviceNo, int status)
        {
            if (Enum.IsDefined(typeof(EMachineUnit), deviceNo))
            {
                EMachineUnit unit = (EMachineUnit)deviceNo;

                switch (unit)
                {
                    case EMachineUnit.Pier1:
                        if (Enum.IsDefined(typeof(EPierStatus), status))
                        {
                            EPierStatus newP1Status = (EPierStatus)status;

                            if (dcMachineUnitStatus[unit].pier1Status != newP1Status)
                            {
                                dcMachineUnitStatus[unit].pier1Status = newP1Status;
                                dgMachineUnitStatus?.Invoke(dcMachineUnitStatus);
                            }
                        }
                        break;

                    case EMachineUnit.Pier2:
                        if(Enum.IsDefined(typeof(EPierStatus), status))
                        {
                            EPierStatus newP2Status = (EPierStatus)status;

                            if (dcMachineUnitStatus[unit].pier2Status != newP2Status)
                            {
                                dcMachineUnitStatus[unit].pier2Status = newP2Status;
                                dgMachineUnitStatus?.Invoke(dcMachineUnitStatus);
                            }
                        }
                        break;

                    case EMachineUnit.Robot:
                        if(Enum.IsDefined(typeof(ERobotStatus), status))
                        {
                            ERobotStatus newRobotStatus = (ERobotStatus)status;

                            if(dcMachineUnitStatus[unit].robotStatus != newRobotStatus)
                            {
                                dcMachineUnitStatus[unit].robotStatus = newRobotStatus;
                                dgMachineUnitStatus?.Invoke(dcMachineUnitStatus);
                            }
                        }
                        break;

                    case EMachineUnit.UPS:
                        if(Enum.IsDefined(typeof(EUpsStatus), status))
                        {
                            EUpsStatus newUpsStatus = (EUpsStatus)status;

                            if(dcMachineUnitStatus[unit].upsStatus != newUpsStatus)
                            {
                                dcMachineUnitStatus[unit].upsStatus = newUpsStatus;
                                dgMachineUnitStatus?.Invoke(dcMachineUnitStatus);
                            }
                        }
                        break;
                }
            }
        }

        public async Task UpdateRobotLog(int robot, List<LogTable> list)
        {
            //throw new NotImplementedException();
        }

        public async Task UpdateRobotMission(int pier, RobotMissionTable table)
        {
            //throw new NotImplementedException();
        }

        public async Task UpdatePierLog(int pier, List<LogTable> list)
        {
            //throw new NotImplementedException();
        }

        public async Task UpdatePierMission(int pier, PierMissionTable table)
        {
            //throw new NotImplementedException();
        }

        public async Task UpdateMainLog(List<LogTable> list)
        {
            //throw new NotImplementedException();
        }

        public async Task UpdateMissionAssignLog(int pier, List<LogTable> list)
        {
            //throw new NotImplementedException();
        }

        public async Task UpdateWarehouseInform(int pier, WarehouseInform warehouse)
        {
            //throw new NotImplementedException();
        }
    }
}
