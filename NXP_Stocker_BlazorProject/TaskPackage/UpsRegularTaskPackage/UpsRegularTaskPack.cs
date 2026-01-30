using CommonLibraryB_NXP.Library.UPS;
using CommonLibraryB_NXP.Library.UPS.Adapter;
using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.TaskPackage.UpsRegularTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.TaskPackage.UpsRegularTaskPackage
{

    public partial class UpsRegularTaskPack<EUPS>
    {
        readonly EUPS ups;

        readonly IUpsOperate<EUPS> IUpsOP;
        readonly UpsLibrary<EUPS> upsLib;

        readonly IUpsRegularDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IUpsRegularUIObserverable IUpsRegularObser;

        public UpsRegularTaskPack(EUPS ups, UpsLibrary<EUPS> upsLib, UpsRegularDataService dataService, ObserverService observerService)
        {
            this.ups = ups;

            this.IUpsOP = upsLib;
            this.upsLib = upsLib;

            this.IDataService = dataService;

            this.INLogObser = observerService;
            this.IUpsRegularObser = observerService;
        }

        async Task writeNLogError(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Error, log);
        }

        async Task writeNLogInform(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Info, log);
        }
    }

    public partial class UpsRegularTaskPack<EUPS> : IUpsRegularTaskPack
    {
        public async Task<bool> GetUpsNo()
        {
            if(await IUpsOP.GetDeviceNo(ups))
            {
                IDataService.UpsNo = upsLib.Packages[ups].property.upsNo;
                return true;
            }
            else
            {
                string nlog = upsLib.Packages[ups].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> GetUpsStatus()
        {
            if(await IUpsOP.GetUpsStatus(ups))
            {
                IDataService.UpsInform.InputVoltage = upsLib.Packages[ups].property.InputVoltage;
                IDataService.UpsInform.InputFaultVoltage = upsLib.Packages[ups].property.InputFaultVoltage;
                IDataService.UpsInform.OutputVoltage = upsLib.Packages[ups].property.OutputVoltage;
                IDataService.UpsInform.OutputLoad = upsLib.Packages[ups].property.OutputLoad;
                IDataService.UpsInform.InputFrequency = upsLib.Packages[ups].property.InputFrequency;
                IDataService.UpsInform.BatteryVoltage = upsLib.Packages[ups].property.BatteryVoltage;
                IDataService.UpsInform.Temperature = upsLib.Packages[ups].property.Temperature;

                IDataService.UpsInform.UtilityFail = upsLib.Packages[ups].property.UtilityFail;
                IDataService.UpsInform.BatteryLow = upsLib.Packages[ups].property.BatteryLow;
                IDataService.UpsInform.BypassBoostActive = upsLib.Packages[ups].property.BypassBoostActive;
                IDataService.UpsInform.UpsFault = upsLib.Packages[ups].property.UpsFault;
                IDataService.UpsInform.UpsType = upsLib.Packages[ups].property.UpsType;
                IDataService.UpsInform.TestInProgress = upsLib.Packages[ups].property.TestInProgress;
                IDataService.UpsInform.ShutdownActive = upsLib.Packages[ups].property.ShutdownActive;

                return true;
            }
            else
            {
                string nlog = upsLib.Packages[ups].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task UpdateUpsStatus()
        {
            await IUpsRegularObser.NotifyUpsStatusInform(IDataService.UpsInform);
        }

        public async Task UpdateUIUpsRunning()
        {
            await IUpsRegularObser.NotifyUpsAction(4, 905);
        }
    }
}
