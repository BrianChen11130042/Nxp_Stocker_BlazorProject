using CommonLibraryB.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.TaskPackage.MainTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.TaskPackage.MainTaskPackage
{
    public partial class MainTaskPack<EPLC>
    {
        readonly EPLC Pier1;
        readonly EPLC Pier2;
        readonly EPLC Robot;

        readonly IPlcOperate<EPLC> IPeirOp;
        readonly PlcLibrary<EPLC> pierLib;

        readonly IMainDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IMainUIObserverable IMainObser;

        public MainTaskPack(EPLC pier1, EPLC pier2, EPLC Robot, PlcLibrary<EPLC> pierLib,
                            MainDataService dataService, ObserverService observerService)
        {
            this.Pier1 = pier1;
            this.Pier2 = pier2;
            this.Robot = Robot;

            this.IPeirOp = pierLib;
            this.pierLib = pierLib;

            this.IDataService = dataService;

            this.INLogObser = observerService;
            this.IMainObser = observerService;
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

    public partial class MainTaskPack<EPLC> : IMainTaskPack
    {
        public async Task<bool> CheckDbConnect()
        {
            string temp = "開始初始化";

            if(await IDataService.AddLogByMainTask(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int heartBeat { get; set; } = 0;

        ushort _getHeartBeat()
        {
            if(heartBeat == 0)
            {
                heartBeat = 1;
            }
            else
            {
                heartBeat = 0;
            }

            return (ushort)heartBeat;
        }

        public async Task<bool> CheckPlcConnect()
        {
            pierLib.Packages[Robot].property.setRobot.heartBeat = _getHeartBeat();

            if (await IPeirOp.SetHeartBeat(Robot))
            {
                return true;
            }
            else
            {
                string nlog = pierLib.Packages[Robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> SetLogInitSuccess()
        {
            string temp = "初始化成功";

            if (await IDataService.AddLogByMainTask(info, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SetLogInitFail()
        {
            string temp = "初始化失敗";

            if (await IDataService.AddLogByMainTask(err, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateUIInitSuccess()
        {
            await IMainObser.NotifyPopUpMessage(true, "初始化成功");
        }

        public async Task UpdateUIInitFail()
        {
            await IMainObser.NotifyPopUpMessage(true, "初始化失敗");
        }
    }
}
