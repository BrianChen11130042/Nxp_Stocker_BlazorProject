using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{
    public partial class RobotMissionTableLibrary
    {
        readonly IServiceProvider serviceProvider;

        public RobotMissionTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }

    public partial class RobotMissionTableLibrary : IRobotMissionTableOperate
    {
        public Task<(bool status, string msg, RobotMissionTable table)> AddRobotMission(RobotMissionTable data)
        {
            throw new NotImplementedException();
        }

        public Task<(bool status, string msg, RobotMissionTable table)> GetNewRobotMission()
        {
            throw new NotImplementedException();
        }

        public Task<(bool status, string msg, RobotMissionTable table)> GetTargetRobotMission(RobotMissionTable data)
        {
            throw new NotImplementedException();
        }

        public Task<(bool status, string msg, RobotMissionTable table)> UpdateRobotMission(RobotMissionTable data)
        {
            throw new NotImplementedException();
        }
    }

}
