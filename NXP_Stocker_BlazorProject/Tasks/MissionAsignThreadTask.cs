using CommonLibraryB_NXP.Base.FiniteStateMachine;
using DevExpress.Utils.Filtering.Internal;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class MissionAsignThreadTask
    {
        readonly MissionAsignTask pier1AsignTask;
        readonly MissionAsignTask pier2AsignTask;

        public MissionAsignThreadTask(MissionAsignTask pier1AsignTask, MissionAsignTask pier2AsignTask)
        {
            this.pier1AsignTask = pier1AsignTask;
            this.pier2AsignTask = pier2AsignTask;

            interval = 1;
        }
    }

    public enum EMissionAsignThread
    {
        None,
        MissionAsign
    }

    public partial class MissionAsignThreadTask : FSMBase<EMissionAsignThread, int>
    {

        public async override Task Init()
        {
            pier1AsignTask.Set(ES1.Action, EMissionAssign.CheckMission, 0);
            pier2AsignTask.Set(ES1.Action, EMissionAssign.CheckMission, 0);

            Set(ES1.Action, EMissionAsignThread.MissionAsign, 0);
        }

        public async override Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case EMissionAsignThread.None:
                    Set(ES1.Finish, EMissionAsignThread.None, 0);
                    break;

                case EMissionAsignThread.MissionAsign:
                    switch(S3)
                    {
                        case 0:
                            await pier1AsignTask.Run();

                            if (pier1AsignTask.key == EHandshakeKey.Finish)
                            {
                                if (pier1AsignTask.isError)
                                {
                                    SaveState();

                                    isError = true;
                                }
                            }

                            Set(10);

                            break;

                        case 10:
                            await pier2AsignTask.Run();

                            if (pier2AsignTask.key == EHandshakeKey.Finish)
                            {
                                if (pier2AsignTask.isError)
                                {
                                    SaveState();

                                    isError = true;
                                }
                            }

                            Set(0);

                            break;
                    }
                    break;
            }
        }

        public async override Task Error()
        {
            //throw new NotImplementedException();
        }

        public async override Task Finish()
        {
            //throw new NotImplementedException();
        }

        public async override Task Idle()
        {
            //throw new NotImplementedException();
        }
    }
}
