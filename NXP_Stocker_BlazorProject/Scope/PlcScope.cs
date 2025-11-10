using CommonLibraryB_NXP.Library.PLC;
using CommonLibraryB_NXP.Library.PLC.Config;
using CommonLibraryB_NXP.Library.PLC.Property;
using Microsoft.Extensions.DependencyInjection;
using NXP_Stocker_BlazorProject.DeviceName.PLC;

namespace NXP_Stocker_BlazorProject.Scope
{
    public partial class Scope
    {
        public PlcConfigManager<EPLC> plcConfig;
        public PlcPropertyManager<EPLC> plcProperty;
        public PlcLibrary<EPLC> plcLibrary;

        void createPlc()
        {
            plcConfig = provider.GetRequiredService<PlcConfigManager<EPLC>>();
            plcProperty = provider.GetRequiredService<PlcPropertyManager<EPLC>>();
            plcLibrary = provider.GetRequiredService<PlcLibrary<EPLC>>();
        }

        void initPlc()
        {
            plcLibrary.InitPackage();
            plcLibrary.InitAdapter();
        }
    }
}
