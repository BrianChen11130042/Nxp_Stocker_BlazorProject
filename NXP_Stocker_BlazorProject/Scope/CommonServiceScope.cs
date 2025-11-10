using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Observer;

namespace NXP_Stocker_BlazorProject.Scope
{
    public partial class Scope
    {
        public ObserverService observerService;

        public MainDataService mainDataService;

        public MissionAsignDataService pier1MissionAsignDataService;
        public MissionAsignDataService pier2MissionAsignDataService;

        public PierDataService pier1DataService;
        public PierDataService pier2DataService;

        public RobotDataService robotDataService;

        void createCommonService()
        {
            observerService = provider.GetRequiredService<ObserverService>();

            mainDataService = new MainDataService(ILogTableOp, observerService);

            pier1MissionAsignDataService = new MissionAsignDataService(ILogTableOp, IMissionAsignTableOp,
                                                                       IPierMissionTableOp, IRobotMissionTalbeOp,
                                                                       IWarehouseTableOp, observerService);

            pier2MissionAsignDataService = new MissionAsignDataService(ILogTableOp, IMissionAsignTableOp,
                                                                       IPierMissionTableOp, IRobotMissionTalbeOp,
                                                                       IWarehouseTableOp, observerService);

            pier1DataService = new PierDataService(ILogTableOp, IPierMissionTableOp, observerService);

            pier2DataService = new PierDataService(ILogTableOp, IPierMissionTableOp, observerService);

            robotDataService = new RobotDataService(ILogTableOp, IRobotMissionTalbeOp, observerService);
        }

        void initCommonService()
        {
            observerService.AddNLogWritterObserver(logger);
        }
    }
}
