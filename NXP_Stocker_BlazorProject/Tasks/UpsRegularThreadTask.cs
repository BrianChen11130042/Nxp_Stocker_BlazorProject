using CommonLibraryB_NXP.Base.FiniteStateMachine;

namespace NXP_Stocker_BlazorProject.Tasks
{

    public partial class UpsRegularThreadTask
    {
        readonly UpsRegularTask upsRegularTask;

        public UpsRegularThreadTask(UpsRegularTask upsRegularTask)
        {
            this.upsRegularTask = upsRegularTask;

            interval = 1;
        }
    }

    public enum EUpsRegularThread
    {
        None,
        UpsRegular
    }

    public partial class UpsRegularThreadTask : FSMBase<EUpsRegularThread, int>
    {
        public async override Task Init()
        {
            upsRegularTask.Set(ES1.Action, EUpsRegular.UpsStatus, 0);

            Set(ES1.Action, EUpsRegularThread.UpsRegular, 0);
        }

        public async override Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case EUpsRegularThread.None:
                    Set(ES1.Finish, EUpsRegularThread.None, 0);
                    break;

                case EUpsRegularThread.UpsRegular:
                    switch(S3)
                    {
                        case 0:
                            await upsRegularTask.Run();

                            if(upsRegularTask.key == EHandshakeKey.Finish)
                            {
                                if(upsRegularTask.isError)
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
