using CommonLibraryB_NXP.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage
{
    public partial class MissionAsignTaskPack<EPLC>
    {
        readonly EPLC pier;

        readonly IPlcOperate<EPLC> IPeirOp;

        readonly PlcLibrary<EPLC> pierLib;

        readonly IMissionAsignDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IMissionAssignUIObserverable IMissionAsignObser;

        public MissionAsignTaskPack(EPLC pier, PlcLibrary<EPLC> pierLib,
                                    MissionAsignDataService dataService, ObserverService observerService)
        {
            this.pier = pier;
            this.IPeirOp = pierLib;
            this.pierLib = pierLib;

            this.IDataService = dataService;
            this.INLogObser = observerService;
            this.IMissionAsignObser = observerService;
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

        Dictionary<int, string> dcMissionAssign { get; set; } = new Dictionary<int, string>()
        {
            { 0, "NoData"},
            { 1, "入庫"},
            { 2, "出庫"},
            { 3, "儲位轉移"}
        };

        Dictionary<int, string> dcBoardSize { get; set; } = new Dictionary<int, string>()
        {
            { 0, "大板"},
            { 1, "小板"}
        };

    }

    public partial class MissionAsignTaskPack<EPLC> : IMissionAsignTaskPack
    {

        public async Task<bool> GetPlcPierName()
        {
            if (await IPeirOp.GetDeviceName(pier))
            {
                IDataService.PierName = pierLib.Packages[pier].property.getPier.pierName;
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        int _reset { get; set; } = 0;

        public async Task<bool> GetPlcIsReset()
        {
            if(await IPeirOp.GetDeviceIsReady(pier))
            {
                _reset = pierLib.Packages[pier].property.getPier.isReady;
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[pier].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public bool IsPlcReset()
        {
            if(_reset == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> GetTableNewMissionAsign()
        {
            if(await IDataService.GetNewMissionAsignTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> GetTableWarehousePickPort()
        {
            if (await IDataService.GetWarehousePickTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> GetTableWarehouseDropPort()
        {
            if(await IDataService.GetWarehouseDropTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTableNewPierMission()
        {
            int pierActionCode = getPierActionCode(IDataService.MissionAsign.ActionCode, 
                                                   IDataService.MissionAsign.BoardSize);

            PierMissionTable pier = new PierMissionTable()
            {
                PierName = IDataService.MissionAsign.PierName,
                AsignId = IDataService.MissionAsign.Id,
                Barcode = IDataService.MissionAsign.Barcode,
                ActionCode = pierActionCode,
                EstablishTime = DateTime.Now,
            };

            IDataService.PierMission = pier;

            if(await IDataService.SetNewPierMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int getPierActionCode(int missionAsignActionCode, int boardSize)
        {
            switch(missionAsignActionCode)
            {
                //入庫
                case 1:
                    if(boardSize == 0) //大板
                    {
                        return 1;
                    }
                    else //小板
                    {
                        return 3;
                    }

                //出庫
                case 2:
                    if(boardSize == 0) //大板
                    {
                        return 2;
                    }
                    else //小板
                    {
                        return 4;
                    }

                default:
                    return 0;
            }
        }

        public async Task<bool> GetTablePierMissionStatus()
        {
            if(await IDataService.GetTargetPierMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPierMissionFinish()
        {
            if(IDataService.PierMission.IsFinish == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTableNewRobotMission()
        {
            RobotMissionTable robot = new RobotMissionTable()
            {
                PierName = IDataService.MissionAsign.PierName,
                AsignId = IDataService.MissionAsign.Id,
                Barcode = IDataService.MissionAsign.Barcode,
                BoardSize = IDataService.MissionAsign.BoardSize,
                PickZone = IDataService.MissionAsign.PickZone,
                PickLayer = IDataService.MissionAsign.PickLayer,
                DropZone = IDataService.MissionAsign.DropZone,
                DropLayer = IDataService.MissionAsign.DropLayer,
                EstablishTime = DateTime.Now
            };

            IDataService.RobotMission = robot;

            if(await IDataService.SetNewRobotMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> GetTableRobotMissionStatus()
        {
            if(await IDataService.GetTargetRobotMissionTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRobotMissionError()
        {
            return false; //待跟電控討論
        }

        public bool IsRobotMissionFinish()
        {
            if(IDataService.RobotMission.IsFinish == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTableMissionAsignStart()
        {
            IDataService.MissionAsign.IsStart = true;
            IDataService.MissionAsign.StartTime = DateTime.Now;

            if (await IDataService.SetMissionAsignTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTableMissionAsignFinsih()
        {
            IDataService.MissionAsign.IsFinish = true;
            IDataService.MissionAsign.FinishTime = DateTime.Now;

            if(await IDataService.SetMissionAsignTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetLogMissionAsignStart()
        {
            string temp = IDataService.MissionAsign.PierName 
                          + dcBoardSize[IDataService.MissionAsign.BoardSize]
                          + dcMissionAssign[IDataService.MissionAsign.ActionCode] + "_任務開始";

            if(await IDataService.AddLogByMissionAsign(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetLogMissionAsignFinish()
        {
            string temp = IDataService.MissionAsign.PierName
                         + dcBoardSize[IDataService.MissionAsign.BoardSize]
                         + dcMissionAssign[IDataService.MissionAsign.ActionCode] + "_任務結束";

            if(await IDataService.AddLogByMissionAsign(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateUIMissionAsign()
        {
            await IMissionAsignObser.NotifyMissionAsign(IDataService.PierName, IDataService.MissionAsign);
        }

        public async Task UpdateUIMissionAsignLog()
        {
            await IMissionAsignObser.NotifyMissionAsignLog(IDataService.PierName, IDataService.ListMissionAsignLog);
        }
    }

    public partial class MissionAsignTaskPack<EPLC>
    {
        public async Task<bool> SetTableWarehouseInputPickPort()
        {
            IDataService.PickPort.IsOccupy = true;
            IDataService.PickPort.Barcode = IDataService.MissionAsign.Barcode;
            IDataService.PickPort.BoardSize = IDataService.MissionAsign.BoardSize;

            if(await IDataService.SetWarehousePickTable())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> SetTableWarehouseOutputPickPort()
        {
            IDataService.PickPort.IsOccupy = false;
            IDataService.PickPort.Barcode = string.Empty;
            IDataService.PickPort.BoardSize = 999;

            if(await IDataService.SetWarehousePickTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTableWarehouseInputDropPort()
        {
            IDataService.DropPort.IsOccupy = true;
            IDataService.DropPort.Barcode = IDataService.MissionAsign.Barcode;
            IDataService.DropPort.BoardSize = IDataService.MissionAsign.BoardSize;

            if(await IDataService.SetWarehouseDropTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetTableWarehouseOutputDropPort()
        {
            IDataService.DropPort.IsOccupy = false;
            IDataService.DropPort.Barcode = string.Empty;
            IDataService.DropPort.BoardSize = 999;

            if(await IDataService.SetWarehouseDropTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public partial class MissionAsignTaskPack<EPLC>
    {
        public bool IsInputWarehouse()
        {
            if (!string.IsNullOrEmpty(IDataService.MissionAsign.Barcode)
                && IDataService.MissionAsign.ActionCode == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsOutputWarehouse()
        {
            if(!string.IsNullOrEmpty(IDataService.MissionAsign.Barcode)
                && IDataService.MissionAsign.ActionCode == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTransformWarehouse()
        {
            if (!string.IsNullOrEmpty(IDataService.MissionAsign.Barcode)
                && IDataService.MissionAsign.ActionCode == 3)
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
