using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{

    public partial class MissionAsignTableLibrary
    {

        readonly IServiceProvider serviceProvider;

        public MissionAsignTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }

    public partial class MissionAsignTableLibrary : IMissionAsignTableOperate
    {
        public Task<(bool status, string msg, MissionAsignTable table)> AddMissionAsign(MissionAsignTable data)
        {
            throw new NotImplementedException();
        }

        public Task<(bool status, string msg, MissionAsignTable table)> GetNewMissionAsign(int PierNo)
        {
            throw new NotImplementedException();
        }

        public Task<(bool status, string msg, MissionAsignTable table)> UpdateMissionAsign(MissionAsignTable data)
        {
            throw new NotImplementedException();
        }
    }
}
