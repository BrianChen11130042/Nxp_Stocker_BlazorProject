using CommonLibraryB_NXP.Tools.LogWritter;
using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Adapter;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.TaskPackage.ThreadTaskPackage.Interface;
using CommonLibraryB_NXP.Library.UPS.Adapter;
using CommonLibraryB_NXP.Library.UPS;

namespace NXP_Stocker_BlazorProject.TaskPackage.ThreadTaskPackage
{
    public partial class ThreadTaskPack<EPLC, EUPS>
    {
        readonly EPLC pier1;
        readonly EPLC pier2;
        readonly EPLC robot;

        readonly IPlcOperate<EPLC> IPlcOp;
        readonly PlcLibrary<EPLC> plcLib;

        readonly EUPS ups;
        readonly IUpsOperate<EUPS> IUpsOP;
        readonly UpsLibrary<EUPS> upsLib;

        readonly IMainDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IMainUIObserverable IMainObser;

        public ThreadTaskPack(EPLC pier1, EPLC pier2, EPLC robot, PlcLibrary<EPLC> plcLib,
                              EUPS ups, UpsLibrary<EUPS> upsLib,
                              MainDataService dataService, ObserverService observerService)
        {
            this.pier1 = pier1;
            this.pier2 = pier2;
            this.robot = robot;

            this.IPlcOp = plcLib;
            this.plcLib = plcLib;

            this.ups = ups;
            this.IUpsOP = upsLib;
            this.upsLib = upsLib;

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

    public partial class ThreadTaskPack<EPLC, EUPS> : IThreadTaskPack
    {
        public async Task<bool> InitMissionAsignInQue()
        {
            if(await IDataService.InitMissionAsignByInQue())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckPlcConnect()
        {
            if (await IPlcOp.GetDeviceIsReady(robot))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[robot].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> CheckUpsConnect()
        {
            if(await IUpsOP.GetUpsStatus(ups))
            {
                return true;
            }
            else
            {
                string nlog = upsLib.Packages[ups].errorLog;
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

        public async Task<bool> SetLogConnectFail()
        {
            string temp = "連線失敗";

            if(await IDataService.AddLogByMainTask(err, temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateUIPopInitSuccess()
        {
            await IMainObser.NotifyPopUpMessage(true, "初始化成功");
        }

        public async Task UpdateUIPopInitFail()
        {
            await IMainObser.NotifyPopUpMessage(true, "初始化失敗");
        }

        public async Task UpdateUIPopConnectFail()
        {
            await IMainObser.NotifyPopUpMessage(true, "連線失敗");
        }

        public async Task UpdateUIMainLog()
        {
            await IMainObser.NotifyMainLog(IDataService.ListMainLog);
        }
    }
}
