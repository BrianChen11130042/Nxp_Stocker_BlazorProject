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
            NotifyStartInitial();

            initCommonService();
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

        public async Task ReconnectPlc()
        {
            reconnectModbusTcpManager();
            initPlc();

            await RetrieveAllTaskState();
        }

        public async Task ReconnectUps()
        {
            reconnectModbusRtuManager();
            initUps();

            await RetrieveAllTaskState();
        }

        public async Task ReconnectAll()
        {
            reconnectModbusTcpManager();
            reconnectModbusRtuManager();

            initPlc();
            initUps();

            await RetrieveAllTaskState();
        }

        public async Task RetrieveAllTaskState()
        {
            pier1Task.RetrieveState();
            pier2Task.RetrieveState();
            robotTask.RetrieveState();

            pier1MissionAsignTask.RetrieveState();
            pier2MissionAsignTask.RetrieveState();

            plcRegularTask.RetrieveState();
            upsRegularTask.RetrieveState();

            missionAsignThreadTask.RetrieveState();
            missionThreadTask.RetrieveState();
            plcRegularThreadTask.RetrieveState();
            upsRegularThreadTask.RetrieveState();
            mainThreadTask.RetrieveState();
        }
    }
}
