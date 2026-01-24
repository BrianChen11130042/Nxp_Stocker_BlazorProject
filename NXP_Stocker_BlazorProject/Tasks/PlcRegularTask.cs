using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.PlcRegularTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class PlcRegularTask : IPlcRegularTaskPack
    {
        readonly IPlcRegularTaskPack pack;

        public PlcRegularTask(IPlcRegularTaskPack pack)
        {
            this.pack = pack;
            interval = 1;
        }

        public Task<bool> GetPlcWarehouse()
        {
            return pack.GetPlcWarehouse();
        }

        public Task<bool> SetPlcHeartBeat()
        {
            return pack.SetPlcHeartBeat();
        }

        public Task UpdateUIWarehouse()
        {
            return pack.UpdateUIWarehouse();
        }
    }

    public enum EPlcRegular
    {
        None,
        HeartBeat,
        Warehouse,
    }

    public partial class PlcRegularTask : FSMBase<EPlcRegular, int>
    {
        public async override Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case EPlcRegular.None:
                    Set(ES1.Finish, EPlcRegular.None, 0);
                    break;

                case EPlcRegular.HeartBeat:
                    switch(S3)
                    {
                        case 0:
                            if(await SetPlcHeartBeat())
                            {
                                Set(EPlcRegular.Warehouse, 0);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPlcRegular.None, 0);
                            }
                            break;
                    }
                    break;

                case EPlcRegular.Warehouse:
                    switch(S3)
                    {
                        case 0:
                            if(await GetPlcWarehouse())
                            {
                                await UpdateUIWarehouse();
                                Set(EPlcRegular.HeartBeat, 0);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EPlcRegular.None, 0);
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
            Set(ES1.Idle, EPlcRegular.None, 0);
        }

        public async override Task Finish()
        {
            isError = false;
            key = EHandshakeKey.Finish;
            Set(ES1.Idle, EPlcRegular.None, 0);
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
