using CommonLibraryB.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.RobotTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{

    public partial class RobotTask : IRobotTaskPack
    {
        readonly IRobotTaskPack pack;

        public RobotTask(IRobotTaskPack pack)
        {
            this.pack = pack;
            interval = 10;
        }
    }

    public enum ERobotAction
    {
        None,
        CheckMission,
        Start,
        Result,
        Success,
        Fail,
    }

    public partial class RobotTask : FSMBase<ERobotAction, int>
    {
        public async override Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case ERobotAction.None:
                    Set(ES1.Finish, ERobotAction.None, 0);
                    break;

                case ERobotAction.CheckMission:

                    break;
            }
        }

        public async override Task Error()
        {
            isError = true;
            key = EHandshakeKey.Finish;
            Set(ES1.Idle, ERobotAction.None, 0);
        }

        public async override Task Finish()
        {
            isError = false;
            key = EHandshakeKey.Finish;
            Set(ES1.Idle, ERobotAction.None, 0);
        }

        public async override Task Idle()
        {
            //throw new NotImplementedException();
        }

        public async override Task Init()
        {
            //throw new NotImplementedException();
        }
    }
}
