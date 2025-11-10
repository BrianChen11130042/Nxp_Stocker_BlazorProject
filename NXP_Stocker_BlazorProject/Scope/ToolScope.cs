using CommonLibraryB.Tools.LogWritter;

namespace NXP_Stocker_BlazorProject.Scope
{

    public partial class MachineScope
    {
        public LogWritter logger;

        void createTool()
        {
            logger = provider.GetRequiredService<LogWritter>();
        }
    }
}
