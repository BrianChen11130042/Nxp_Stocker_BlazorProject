using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{
    public partial class PierMissionTableLibrary
    {
        readonly IServiceProvider serviceProvider;

        public PierMissionTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }

    public partial class PierMissionTableLibrary : IPierMissionTableOperate
    {
        public Task<(bool status, string msg, PierMissionTable table)> AddPierMission(PierMissionTable data)
        {
            throw new NotImplementedException();
        }

        public Task<(bool status, string msg, PierMissionTable table)> GetNewPierMission(int PierNo)
        {
            throw new NotImplementedException();
        }

        public Task<(bool status, string msg, PierMissionTable table)> GetTargetPierMission(PierMissionTable data)
        {
            throw new NotImplementedException();
        }

        public Task<(bool status, string msg, PierMissionTable table)> UpdatePierMission(PierMissionTable data)
        {
            throw new NotImplementedException();
        }
    }
}
