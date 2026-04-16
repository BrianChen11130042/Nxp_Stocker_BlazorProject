using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.UpsRegularTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{

    public partial class UpsRegularTask : IUpsRegularTaskPack
    {
        readonly IUpsRegularTaskPack pack;

        public UpsRegularTask(IUpsRegularTaskPack pack)
        {
            this.pack = pack;
            interval = 1;
        }

        public Task<bool> GetUpsNo()
        {
            return pack.GetUpsNo();
        }

        public Task<bool> GetUpsStatus()
        {
            return pack.GetUpsStatus();
        }

        public Task UpdateUIUpsDisconnect()
        {
            return pack.UpdateUIUpsDisconnect();
        }

        public Task UpdateUIUpsRunning()
        {
            return pack.UpdateUIUpsRunning();
        }

        public Task UpdateUpsStatus()
        {
            return pack.UpdateUpsStatus();
        }
    }

    public enum EUpsRegular
    {
        None,
        UpsStatus
    }

    public partial class UpsRegularTask : FSMBase<EUpsRegular, int>
    {
        public async override Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case EUpsRegular.None:
                    Set(ES1.Finish, EUpsRegular.None, 0);
                    break;

                case EUpsRegular.UpsStatus:
                    switch(S3)
                    {
                        case 0:
                            await GetUpsNo();

                            if (await GetUpsStatus())
                            {
                                await UpdateUIUpsRunning();
                                await UpdateUpsStatus();
                                Set(0);
                            }
                            else
                            {
                                await UpdateUIUpsDisconnect();

                                SaveState();
                                Set(ES1.Error, EUpsRegular.None, 0);
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
            Set(ES1.Idle, EUpsRegular.None, 0);
        }

        public async override Task Finish()
        {
            isError = false;
            key = EHandshakeKey.Finish;
            Set(ES1.Idle, EUpsRegular.None, 0);
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
