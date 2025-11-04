using CommonLibraryB.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class MissionAssignTask : IMissionAsignTaskPack
    {
        readonly IMissionAsignTaskPack pack;

        public MissionAssignTask(IMissionAsignTaskPack pack)
        {
            this.pack = pack;
            interval = 10;
        }

        
    }

    public enum EMission
    {
        None,
        CheckMission,
        InputWarehouse,
        OutputWarehouse,
        TransformWarehouse
    }

    public partial class MissionAssignTask : FSMBase<EMission, int>
    {
        public override Task Action()
        {
            throw new NotImplementedException();
        }

        public override Task Error()
        {
            throw new NotImplementedException();
        }

        public override Task Finish()
        {
            throw new NotImplementedException();
        }

        public override Task Idle()
        {
            throw new NotImplementedException();
        }

        public override Task Init()
        {
            throw new NotImplementedException();
        }
    }
}
