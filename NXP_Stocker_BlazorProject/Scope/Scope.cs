namespace NXP_Stocker_BlazorProject.Scope
{
    
    public partial class Scope
    {
        IServiceProvider provider;

        public Scope(IServiceProvider provider)
        {
            this.provider = provider;
            createAll();
        }

        public void createAll()
        {
            createTool();
            createCommonService();
            createManager();
            createPlc();
        }

        public void initAll()
        {

        }
    }
}
