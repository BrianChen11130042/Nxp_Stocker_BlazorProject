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
        public async Task UpdateMainLog(List<LogTable> list)
        {
            //throw new NotImplementedException();
        }

        public event dgInitMessage dgInitMsg;

        public async Task UpdatePopUpMessage(bool popUp, string msg)
        {
            dgInitMsg?.Invoke(popUp, msg);
        }
    }

    public delegate Task dgPlcActionStatus(Dictionary<EPLC, bool> dcPlcAction);

    public partial class MachineService : IPierUIObserver
    {
        public event dgPlcActionStatus dgPlcAction;

        Dictionary<EPLC, bool> dcPlcAction { get; set; } = new Dictionary<EPLC, bool>()
        {
            { EPLC.Pier1, false},
            { EPLC.Pier2, false},
            { EPLC.Robot, false}
        };

        public async Task<Dictionary<EPLC, bool>> GetDcPlcAction()
        {
            return dcPlcAction;
        }

        public async Task UpdatePierAction(int pier, bool isRun)
        {
            switch(pier)
            {
                case 1:
                    if (dcPlcAction[EPLC.Pier1] != isRun)
                    {
                        dcPlcAction[EPLC.Pier1] = isRun;
                        dgPlcAction?.Invoke(dcPlcAction);
                    }
                    break;  

                case 2:
                    if(dcPlcAction[EPLC.Pier2] != isRun)
                    {
                        dcPlcAction[EPLC.Pier2] = isRun;
                        dgPlcAction?.Invoke(dcPlcAction);
                    }
                    break;

                default:
                    break;
            }
        }

        public async Task UpdatePierLog(int pier, List<LogTable> list)
        {
            //throw new NotImplementedException();
        }

        public async Task UpdatePierMission(int pier, PierMissionTable table)
        {
            //throw new NotImplementedException();
        }
    }

    public partial class MachineService : IRobotUIObserver
    {
        public async Task UpdateRobotAction(int robot, bool isRun)
        {
            if (dcPlcAction[EPLC.Robot] != isRun)
            {
                dcPlcAction[EPLC.Robot] = isRun;
                dgPlcAction?.Invoke(dcPlcAction);
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
}
