using CommonLibraryB_NXP.Library.UPS;
using CommonLibraryB_NXP.Library.UPS.Config;
using CommonLibraryB_NXP.Library.UPS.Property;
using NXP_Stocker_BlazorProject.DeviceName.UPS;

namespace NXP_Stocker_BlazorProject.Scope
{

    public partial class MachineScope
    {
        public UpsConfigManager<EUPS> upsConfig;
        public UpsPropertyManager<EUPS> upsProperty;
        public UpsLibrary<EUPS> upsLibrary;

        void createUps()
        {
            upsConfig = provider.GetRequiredService<UpsConfigManager<EUPS>>();
            upsProperty = provider.GetRequiredService<UpsPropertyManager<EUPS>>();
            upsLibrary = provider.GetRequiredService<UpsLibrary<EUPS>>();
        }

        void initUps()
        {
            upsLibrary.InitPackage();
            upsLibrary.InitAdapter();
        }

    }
}
