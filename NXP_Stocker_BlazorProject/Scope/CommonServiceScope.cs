using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Observer;

namespace NXP_Stocker_BlazorProject.Scope
{
    public partial class MachineScope
    {
        public ObserverService observerService;

        public MainDataService mainDataService;

        public MissionAsignDataService pier1MissionAsignDataService;
        public MissionAsignDataService pier2MissionAsignDataService;

        public PierDataService pier1DataService;
        public PierDataService pier2DataService;

        public RobotDataService robotDataService;

        public PlcRegularDataService plcRegularDataService;

        public UpsRegularDataService upsRegularDataService;

        void createCommonService()
        {
            observerService = provider.GetRequiredService<ObserverService>();

            mainDataService = new MainDataService(ILogTableOp, IMissionTableOp, observerService);

            pier1MissionAsignDataService = new MissionAsignDataService(ILogTableOp, IMissionTableOp,
                                                                       observerService);

            pier2MissionAsignDataService = new MissionAsignDataService(ILogTableOp, IMissionTableOp,
                                                                       observerService);

            pier1DataService = new PierDataService(ILogTableOp, IMissionTableOp, observerService);

            pier2DataService = new PierDataService(ILogTableOp, IMissionTableOp, observerService);

            robotDataService = new RobotDataService(ILogTableOp, IMissionTableOp, observerService);

            plcRegularDataService = new PlcRegularDataService(observerService);

            upsRegularDataService = new UpsRegularDataService(observerService);
        }

        void initCommonService()
        {
            observerService.AddNLogWritterObserver(logger);
        }
    }
}
