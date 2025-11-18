using Microsoft.Extensions.DependencyInjection;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.Scope
{

    public partial class MachineScope
    {
        public ILogTableOperate ILogTableOp;
        public IMissionTableOperate IMissionAsignTableOp;
        public IPierMissionTableOperate IPierMissionTableOp;
        public IRobotMissionTableOperate IRobotMissionTalbeOp;
        public IWarehouseTableOperate IWarehouseTableOp;

        void createDbTable()
        {
            ILogTableOp = provider.GetRequiredService<ILogTableOperate>();
            IMissionAsignTableOp = provider.GetRequiredService<IMissionTableOperate>();
            IPierMissionTableOp = provider.GetRequiredService<IPierMissionTableOperate>();
            IRobotMissionTalbeOp = provider.GetRequiredService<IRobotMissionTableOperate>();
            IWarehouseTableOp = provider.GetRequiredService<IWarehouseTableOperate>();
        }
    }
}
