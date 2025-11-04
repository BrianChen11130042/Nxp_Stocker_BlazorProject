using CommonLibraryB.Tools.LogWritter;
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

        readonly IPlcOperate<EPLC> IRobotOp;

        readonly PlcLibrary<EPLC> RobotLib;

        readonly IRobotDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IRobotUIObserverable IRobotObser;

        public RobotTaskPack(EPLC robot, PlcLibrary<EPLC> robotLib,
                             RobotDataService dataService, ObserverService observerService)
        {
            this.robot = robot;
            this.IRobotOp = robotLib;
            this.RobotLib = robotLib;

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

        int robotStatus { get; set; } = 0;
    }

    public partial class RobotTaskPack<EPLC> : IRobotTaskPack
    {

        //目前電控還沒給規格 先放著

        public async Task<bool> GetRobotStatus()
        {
            return true;
        }

        public bool IsRobotError()
        {
            return false;
        }

        ///////////////////////////
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
            if (!string.IsNullOrEmpty(IDataService.PierName)
               && !string.IsNullOrEmpty(IDataService.RobotMission.MissionSerialNumber)
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
            RobotLib.Packages[robot].property.setRobot.barcode = IDataService.RobotMission.Barcode;
            RobotLib.Packages[robot].property.setRobot.boardSize = (ushort)IDataService.RobotMission.BoardSize;
            RobotLib.Packages[robot].property.setRobot.pickZone = (ushort)IDataService.RobotMission.PickZone;
            RobotLib.Packages[robot].property.setRobot.pickLayer = (ushort)IDataService.RobotMission.PickLayer;
            RobotLib.Packages[robot].property.setRobot.dropZone = (ushort)IDataService.RobotMission.DropZone;
            RobotLib.Packages[robot].property.setRobot.dropLayer = (ushort)IDataService.RobotMission.DropLayer;

            if (await IRobotOp.SetRobotMissionInform(robot))
            {
                return true;
            }
            else
            {
                string nlog = RobotLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> SetPlcRobotStart()
        {
            RobotLib.Packages[robot].property.setRobot.missionStart = 1;

            if (await IRobotOp.SetRobotMissionStart(robot))
            {
                return true;
            }
            else
            {
                string nlog = RobotLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> SetTableMissionStart()
        {
            IDataService.RobotMission.IsStart = true;
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
            if(await IRobotOp.GetRobotStatus(robot))
            {
                robotStatus = RobotLib.Packages[robot].property.getRobot.missionStatus;
                IDataService.RobotMission.Status = robotStatus.ToString();
                return true;
            }
            else
            {
                string nlog = RobotLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsRobotFinish()
        {
            if(robotStatus == 0)
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
            RobotLib.Packages[robot].property.setRobot.missionFinish = 0;

            if(await IRobotOp.SetRobotMissionFinsih(robot))
            {
                return true;
            }
            else
            {
                string nlog = RobotLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> SetTableMissionFinish()
        {
            IDataService.RobotMission.IsFinish = true;
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
            string temp = IDataService.PierName + "區手臂任務開始";

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
            string temp = IDataService.PierName + "區手臂任務結束";

            if (await IDataService.AddLogByRobot(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateUIRobotMission()
        {
            await IRobotObser.NotifyRobotMission(IDataService.PierName, IDataService.RobotMission);
        }

        public async Task UpdateUIRobotLog()
        {
            await IRobotObser.NotifyRobotLog(IDataService.PierName, IDataService.ListRobotLog);
        }
    }
}
