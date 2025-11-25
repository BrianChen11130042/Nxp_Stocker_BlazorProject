using NXP_Stocker_BlazorProject.TaskPackage.ThreadTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class MissionThreadTask : IThreadTaskPack
    {
        readonly IThreadTaskPack pack;

        public MissionThreadTask()
        {

        }

        public Task<bool> CheckPlcConnect()
        {
            return pack.CheckPlcConnect();
        }

        public Task<bool> InitMissionAsignInQue()
        {
            return pack.InitMissionAsignInQue();
        }

        public Task<bool> SetLogConnectFail()
        {
            return pack.SetLogConnectFail();
        }

        public Task<bool> SetLogInitFail()
        {
            return pack.SetLogInitFail();
        }

        public Task<bool> SetLogInitSuccess()
        {
            return pack.SetLogInitSuccess();
        }

        public Task<bool> SetPlcHeartBeat()
        {
            return pack.SetPlcHeartBeat();
        }

        public Task UpdateUIMainLog()
        {
            return pack.UpdateUIMainLog();
        }

        public Task UpdateUIPopConnectFail()
        {
            return pack.UpdateUIPopConnectFail();
        }

        public Task UpdateUIPopInitFail()
        {
            return pack.UpdateUIPopInitFail();
        }

        public Task UpdateUIPopInitSuccess()
        {
            return pack.UpdateUIPopInitSuccess();
        }
    }
}
