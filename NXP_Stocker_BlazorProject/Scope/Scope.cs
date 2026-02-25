namespace NXP_Stocker_BlazorProject.Scope
{
    
    public partial class MachineScope
    {
        IServiceProvider provider;

        public MachineScope(IServiceProvider provider)
        {
            this.provider = provider;
            createAll();
        }

        public void createAll()
        {
            createTool();
            createDbTable();
            createCommonService();
            createManager();
            createPlc();
            createUps();
        }

        public void initAll()
        {
            StopThread();
            notifyInitUnitStatus();
            initCommonService();
            notifyConnectingUnitStatus();
            initManager();
            initPlc();
            initUps();
            initPierTask();
            initRobotTask();
            initMissionAsignTask();
            initRegularTask();
            initThreadTask();
            initThread();
        }
    }
}
