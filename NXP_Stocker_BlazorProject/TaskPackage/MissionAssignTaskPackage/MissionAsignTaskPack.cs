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
using NXP_Stocker_BlazorProject.MachineModel;
using NXP_Stocker_BlazorProject.Tasks;

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

        public async Task<bool> GetPlcPierNo()
        {
            if (await IPeirOp.GetDeviceNo(pier))
            {
                IDataService.PierNo = pierLib.Packages[pier].property.getPier.pierNo;
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

        public async Task<bool> GetPlcIsReady()
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

        public bool IsPlcReady()
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
            WarehouseInform pick = new WarehouseInform();

            if (IDataService.MissionAsign.ActionCode == 1)
            {
                pick.pierNo = IDataService.MissionAsign.PierNo;
                pick.zone = IDataService.MissionAsign.PickZone;
                pick.layer = IDataService.MissionAsign.PickLayer;

                pick.isOccupy = false;
                pick.barcode = string.Empty;
                pick.size = 0;
            }
            else
            {
                pick.pierNo = IDataService.MissionAsign.PierNo;
                pick.zone = IDataService.MissionAsign.PickZone;
                pick.layer = IDataService.MissionAsign.PickLayer;

                pick.isOccupy = true;
                pick.barcode = IDataService.MissionAsign.Barcode;
                pick.size = IDataService.MissionAsign.BoardSize;
            }


            IDataService.PickPort = pick;

            return true;
        }

        public async Task<bool> GetTableWarehouseDropPort()
        {

            WarehouseInform drop = new WarehouseInform()
            {
                pierNo = IDataService.MissionAsign.PierNo,
                zone = IDataService.MissionAsign.DropZone,
                layer = IDataService.MissionAsign.DropLayer,
                isOccupy = false,
                barcode = string.Empty,
                size = 0
            };

            IDataService.DropPort = drop;

            return true;
        }

        public async Task<bool> SetTableNewPierMission()
        {
            int pierActionCode = getPierActionCode(IDataService.MissionAsign.ActionCode, 
                                                   IDataService.MissionAsign.BoardSize);

            PierMissionTable pier = new PierMissionTable()
            {
                Id = new Guid(),
                AsignId = IDataService.MissionAsign.Id,

                PierNo = IDataService.MissionAsign.PierNo,
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
            if(IDataService.PierMission.FinishTime != null)
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
                Id = new Guid(),
                AsignId = IDataService.MissionAsign.Id,

                PierNo = IDataService.MissionAsign.PierNo,
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
            if(IDataService.RobotMission.FinishTime != null)
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
            string temp = "Pier"
                          + IDataService.MissionAsign.PierNo.ToString()
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
            string temp = "Pier"
                         + IDataService.MissionAsign.PierNo.ToString()
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
            await IMissionAsignObser.NotifyMissionAsign(IDataService.PierNo, IDataService.MissionAsign);
        }

        public async Task UpdateUIMissionAsignLog()
        {
            await IMissionAsignObser.NotifyMissionAsignLog(IDataService.PierNo, IDataService.ListMissionAsignLog);
        }

        public async Task UpdateUIPickPortWarehouse()
        {
            await IMissionAsignObser.NotifyWarehouseInform(IDataService.PierNo, IDataService.PickPort);
        }

        public async Task UpdateUIDropPortWarehouse()
        {
            await IMissionAsignObser.NotifyWarehouseInform(IDataService.PierNo, IDataService.DropPort);
        }
    }

    public partial class MissionAsignTaskPack<EPLC>
    {
        public async Task<bool> SetTableWarehouseInputPickPort()
        {
            IDataService.PickPort.isOccupy = true;
            IDataService.PickPort.barcode = IDataService.MissionAsign.Barcode;
            IDataService.PickPort.size = IDataService.MissionAsign.BoardSize;

            return true;
        }

        public async Task<bool> SetTableWarehouseOutputPickPort()
        {
            IDataService.PickPort.isOccupy = false;
            IDataService.PickPort.barcode = string.Empty;
            IDataService.PickPort.size = 0;

            return true;
        }

        public async Task<bool> SetTableWarehouseInputDropPort()
        {
            IDataService.DropPort.isOccupy = true;
            IDataService.DropPort.barcode = IDataService.MissionAsign.Barcode;
            IDataService.DropPort.size = IDataService.MissionAsign.BoardSize;

            return true;
        }

        public async Task<bool> SetTableWarehouseOutputDropPort()
        {
            IDataService.DropPort.isOccupy = false;
            IDataService.DropPort.barcode = string.Empty;
            IDataService.DropPort.size = 0;

            return true;
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
