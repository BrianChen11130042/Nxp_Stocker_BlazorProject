using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.ThreadTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class MainThreadTask : IThreadTaskPack
    {
        readonly IThreadTaskPack pack;

        readonly MissionAsignThreadTask missionAsignThread;
        readonly MissionThreadTask missionThread;
        readonly PlcRegularThreadTask plcRegularThread;
        readonly UpsRegularThreadTask upsRegularThread;

        public MainThreadTask(IThreadTaskPack pack, MissionAsignThreadTask missionAsignThread, MissionThreadTask missionThread,
                              PlcRegularThreadTask plcRegularThread, UpsRegularThreadTask upsRegularThread)
        {
            this.pack = pack;

            this.missionAsignThread = missionAsignThread;
            this.missionThread = missionThread;
            this.plcRegularThread = plcRegularThread;
            this.upsRegularThread = upsRegularThread;

            interval = 1;
        }

        public Task<bool> InitMissionAsignInQue()
        {
            return pack.InitMissionAsignInQue();
        }

        public Task<bool> CheckPlcConnect()
        {
            return pack.CheckPlcConnect();
        }

        public Task<bool> CheckUpsConnect()
        {
            return pack.CheckUpsConnect();
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

        public Task UpdateUIMainLog()
        {
            return pack.UpdateUIMainLog();
        }
    }

    public enum EMainThread
    {
        None,
        MonitorSubThread
    }

    public partial class MainThreadTask : FSMBase<EMainThread, int>
    {
        public async override Task Init()
        {
            switch(S3)
            {
                case 0:
                    if (await InitMissionAsignInQue())
                    {
                        Set(10);
                    }
                    else
                    {
                        Set(40);
                    }
                    break;

                case 10:
                    if(await CheckPlcConnect())
                    {
                        Set(20);
                    }
                    else
                    {
                        Set(40);
                    }
                    break;

                case 20:
                    if(await CheckUpsConnect())
                    {
                        Set(30);
                    }
                    else
                    {
                        Set(40);
                    }
                    break;

                case 30:
                    if(await SetLogInitSuccess())
                    {
                        await UpdateUIMainLog();
                        await UpdateUIPopInitSuccess();

                        missionAsignThread.Set(ES1.Init, EMissionAsignThread.None, 0);
                        missionThread.Set(ES1.Init, EMissionThread.None, 0);
                        plcRegularThread.Set(ES1.Init, EPlcRegularThread.None, 0);
                        upsRegularThread.Set(ES1.Init, EUpsRegularThread.None, 0);

                        Set(ES1.Action, EMainThread.MonitorSubThread, 0);
                    }
                    else
                    {
                        Set(40);
                    }
                    break;

                case 40:
                    await SetLogInitFail();
                    await UpdateUIMainLog();
                    await UpdateUIPopInitFail();

                    Set(ES1.None, EMainThread.None, 0);
                    break;
            }
        }

        public override async Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case EMainThread.None:
                    Set(ES1.Finish, EMainThread.None, 0);
                    break;

                case EMainThread.MonitorSubThread:
                    switch(S3)
                    {
                        case 0:
                            if (missionAsignThread.key == EHandshakeKey.Finish)
                            {
                                if(missionAsignThread.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMainThread.None, 0);
                                }
                                else
                                {
                                    Set(10);
                                }
                            }
                            else
                            {
                                Set(10);
                            }
                            break;

                        case 10:
                            if(missionThread.key == EHandshakeKey.Finish)
                            {
                                if(missionThread.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMainThread.None, 0);
                                }
                                else
                                {
                                    Set(20);
                                }
                            }
                            else
                            {
                                Set(20);
                            }
                            break;

                        case 20:
                            if(plcRegularThread.key == EHandshakeKey.Finish)
                            {
                                if(plcRegularThread.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMainThread.None, 0);
                                }
                                else
                                {
                                    Set(30);
                                }
                            }
                            else
                            {
                                Set(30);
                            }
                            break;

                        case 30:
                            if(upsRegularThread.key == EHandshakeKey.Finish)
                            {
                                if(upsRegularThread.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMainThread.None, 0);
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

        public override async Task Error()
        {
            switch(S3)
            {
                case 0:
                    await SetLogConnectFail();
                    Set(10);
                    break;

                case 10:
                    await UpdateUIMainLog();
                    await UpdateUIPopConnectFail();

                    key = EHandshakeKey.Finish;
                    Set(ES1.Idle, EMainThread.None, 0);
                    break;
            }
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
