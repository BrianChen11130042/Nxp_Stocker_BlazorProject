using CommonLibraryB_NXP.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using CommonLibraryB_NXP.Library.PLC;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage
{

    public partial class RobotTaskPack<EPLC>
    {
        readonly EPLC robot;

        readonly IPlcOperate<EPLC> IPlcOp;

        readonly PlcLibrary<EPLC> PlcLib;

        readonly IRobotDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IRobotUIObserverable IRobotObser;

        public RobotTaskPack(EPLC robot, PlcLibrary<EPLC> plcLib,
                             RobotDataService dataService, ObserverService observerService)
        {
            this.robot = robot;
            this.IPlcOp = plcLib;
            this.PlcLib = plcLib;

            this.IDataService = dataService;

            this.INLogObser = observerService;
            this.IRobotObser = observerService;
        }

        async Task writeNLogError(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Error, log);
        }

        async Task writeNLogInform(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Info, log);
        }

        string info { get; set; } = "Inform";

        string err { get; set; } = "Error";
    }

    public partial class RobotTaskPack<EPLC> : IRobotTaskPack
    {
        public async Task<bool> GetPlcRobotNo()
        {
            if(await IPlcOp.GetDeviceNo(robot))
            {
                IDataService.RobotNo = PlcLib.Packages[robot].property.getRobot.robotNo;
                return true;
            }
            else
            {
                string nlog = PlcLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        int _reset { get; set; } = 0;

        public async Task<bool> GetRobotIsReady()
        {
            if (await IPlcOp.GetDeviceIsReady(robot))
            {
                _reset = PlcLib.Packages[robot].property.getRobot.isReady;
                return true;
            }
            else
            {
                string nlog = PlcLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsRobotReady()
        {
            if (_reset == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public partial class RobotTaskPack<EPLC>
    {
        public async Task<bool> GetTableNewMission()
        {
            if(await IDataService.GetNewRobotMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsGetNewMission()
        {
            if (IDataService.RobotMission.PierNo != 0
               && !string.IsNullOrEmpty(IDataService.RobotMission.Barcode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcRobotMission()
        {
            PlcLib.Packages[robot].property.setRobot.barcode = IDataService.RobotMission.Barcode;
            PlcLib.Packages[robot].property.setRobot.boardSize = (ushort)IDataService.RobotMission.BoardSize;
            PlcLib.Packages[robot].property.setRobot.pickZone = (ushort)IDataService.RobotMission.PickZone;
            PlcLib.Packages[robot].property.setRobot.pickLayer = (ushort)IDataService.RobotMission.PickLayer;
            PlcLib.Packages[robot].property.setRobot.dropZone = (ushort)IDataService.RobotMission.DropZone;
            PlcLib.Packages[robot].property.setRobot.dropLayer = (ushort)IDataService.RobotMission.DropLayer;

            if (await IPlcOp.SetRobotMissionInform(robot))
            {
                return true;
            }
            else
            {
                string nlog = PlcLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> SetPlcRobotStart()
        {
            PlcLib.Packages[robot].property.setRobot.missionStart = 1;

            if (await IPlcOp.SetRobotMissionStart(robot))
            {
                return true;
            }
            else
            {
                string nlog = PlcLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> SetTableMissionStart()
        {
            //IDataService.RobotMission.IsStart = true;
            IDataService.RobotMission.StartTime = DateTime.Now;

            if(await IDataService.SetRobotMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> GetPlcRobotStatus()
        {
            if(await IPlcOp.GetRobotStatus(robot))
            {
                IDataService.RobotMission.Status = PlcLib.Packages[robot].property.getRobot.missionStatus;
                return true;
            }
            else
            {
                string nlog = PlcLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsRobotFinish()
        {
            if(IDataService.RobotMission.Status == 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetPlcRobotFinish()
        {
            PlcLib.Packages[robot].property.setRobot.missionFinish = 100;

            if(await IPlcOp.SetRobotMissionFinsih(robot))
            {
                return true;
            }
            else
            {
                string nlog = PlcLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> SetTableMissionFinish()
        {
            //IDataService.RobotMission.IsFinish = true;
            IDataService.RobotMission.FinishTime = DateTime.Now;

            if(await IDataService.SetRobotMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetLogMissionStart()
        {
            string temp = "Pier" + IDataService.RobotMission.PierNo.ToString() + "區手臂任務開始";

            if(await IDataService.AddLogByRobot(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetLogMissionFinish()
        {
            string temp = "Pier" + IDataService.RobotMission.PierNo.ToString() + "區手臂任務結束";

            if (await IDataService.AddLogByRobot(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateRobotMissionStatusToInQue()
        {
            await IDataService.UpdateRobotMissionStatusToInque();
        }

        public async Task UpdateUIRobotMission()
        {
            await IRobotObser.NotifyRobotMission(IDataService.RobotNo, IDataService.RobotMission);
        }

        public async Task UpdateUIRobotLog()
        {
            await IRobotObser.NotifyRobotLog(IDataService.RobotNo, IDataService.ListRobotLog);
        }

        public async Task UpdateUIRobotStop()
        {
            await IRobotObser.NotifyRobotAction(IDataService.RobotNo, 907);
        }

        public async Task UpdateUIRobotIdle()
        {
            await IRobotObser.NotifyRobotAction(IDataService.RobotNo, 904);
        }

        public async Task UpdateUIRobotRunning()
        {
            await IRobotObser.NotifyRobotAction(IDataService.RobotNo, 905);
        }
    }
}
