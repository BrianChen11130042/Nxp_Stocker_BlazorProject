using CommonLibraryB_NXP.Base.FiniteStateMachine;

namespace NXP_Stocker_BlazorProject.Tasks
{

    public partial class PlcRegularThreadTask
    {
        readonly PlcRegularTask plcRegularTask;

        public PlcRegularThreadTask(PlcRegularTask plcRegularTask)
        {
            this.plcRegularTask = plcRegularTask;

            interval = 1;
        }
    }

    public enum EPlcRegularThread
    {
        None,
        PlcRegular
    }

    public partial class PlcRegularThreadTask : FSMBase<EPlcRegularThread, int>
    {
        public async override Task Init()
        {
            plcRegularTask.Set(ES1.Action, EPlcRegular.HeartBeat, 0);

            Set(ES1.Action, EPlcRegularThread.PlcRegular, 0);
        }

        public async override Task Action()
        {
            switch(S2)
            {
                case EPlcRegularThread.None:
                    Set(ES1.Finish, EPlcRegularThread.None, 0);
                    break;

                case EPlcRegularThread.PlcRegular:
                    switch(S3)
                    {
                        case 0:
                            await plcRegularTask.Run();

                            if(plcRegularTask.key == EHandshakeKey.Finish)
                            {
                                if(plcRegularTask.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EPlcRegularThread.None, 0);
                                }
                                else
                                {
                                    Set(0);
                                }
                            }
                            else
                            {
                                Set(0);
                            }
                            break;
                    }
                    break;
            }
        }

        public async override Task Error()
        {
            isError = true;
            key = EHandshakeKey.Finish;
            Set(ES1.Idle, EPlcRegularThread.None, 0);
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
