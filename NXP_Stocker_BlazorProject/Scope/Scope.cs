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
        }

        public void initAll()
        {
            initCommonService();
            initManager();
            initPlc();
            initPierTask();
            initRobotTask();
            initMissionAsignTask();
            initMainTask();
            initThread();
        }
    }
}
