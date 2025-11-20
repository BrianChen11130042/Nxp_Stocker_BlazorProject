using Microsoft.Extensions.DependencyInjection;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.Scope
{

    public partial class MachineScope
    {
        public ILogTableOperate ILogTableOp;
        public IMissionTableOperate IMissionTableOp;

        void createDbTable()
        {
            ILogTableOp = provider.GetRequiredService<ILogTableOperate>();
            IMissionTableOp = provider.GetRequiredService<IMissionTableOperate>();
        }
    }
}
