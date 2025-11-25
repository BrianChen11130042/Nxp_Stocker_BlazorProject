using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.ThreadTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class MainThreadTask : IThreadTaskPack
    {
        readonly IThreadTaskPack pack;

        readonly MissionAsignTask pier1AsignTask;
        readonly MissionAsignTask pier2AsignTask;

        readonly PierTask pier1Task;
        readonly PierTask pier2Task;

        readonly RobotTask robotTask;

        public MainThreadTask(IThreadTaskPack pack, MissionAsignTask pier1AsignTask, MissionAsignTask pier2AsignTask,
                              PierTask pier1Task, PierTask pier2Task, RobotTask robotTask)
        {
            this.pack = pack;

            this.pier1AsignTask = pier1AsignTask;
            this.pier2AsignTask = pier2AsignTask;

            this.pier1Task = pier1Task;
            this.pier2Task = pier2Task;

            this.robotTask = robotTask;

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

    public enum EMain
    {
        None,
        HeartBeat,
        MissionAsign,
        Mission,
    }

    public partial class MainThreadTask : FSMBase<EMain, int>
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
                        Set(30);
                    }
                    break;

                case 10:
                    if(await CheckPlcConnect())
                    {
                        Set(20);
                    }
                    else
                    {
                        Set(30);
                    }
                    break;

                case 20:
                    if(await SetLogInitSuccess())
                    {
                        await UpdateUIMainLog();
                        await UpdateUIPopInitSuccess();

                        pier1AsignTask.Set(ES1.Action, EMissionAssign.CheckMission, 0);
                        pier2AsignTask.Set(ES1.Action, EMissionAssign.CheckMission, 0);

                        pier1Task.Set(ES1.Action, EPierAction.CheckMission, 0);
                        pier2Task.Set(ES1.Action, EPierAction.CheckMission, 0);

                        robotTask.Set(ES1.Action, ERobotAction.CheckError, 0);

                        Set(ES1.Action, EMain.HeartBeat, 0);
                    }
                    else
                    {
                        Set(30);
                    }
                    break;

                case 30:
                    await SetLogInitFail();
                    await UpdateUIMainLog();
                    await UpdateUIPopInitFail();

                    Set(ES1.None, EMain.None, 0);
                    break;
            }
        }

        public override async Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case EMain.None:
                    Set(ES1.Finish, EMain.None, 0);
                    break;

                case EMain.HeartBeat:
                    switch(S3)
                    {
                        case 0:
                            if(await SetPlcHeartBeat())
                            {
                                Set(EMain.MissionAsign, 0);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMain.None, 0);
                            }
                            break;  
                    }
                    break;

                case EMain.MissionAsign:
                    switch(S3)
                    {
                        case 0:
                            await pier1AsignTask.Run();

                            if(pier1AsignTask.key == EHandshakeKey.Finish)
                            {
                                if(pier1AsignTask.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMain.None, 0);
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
                            await pier2AsignTask.Run();

                            if(pier2AsignTask.key == EHandshakeKey.Finish)
                            {
                                if(pier2AsignTask.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMain.None, 0);
                                }
                                else
                                {
                                    Set(EMain.Mission, 0);
                                }
                            }
                            else
                            {
                                Set(EMain.Mission, 0);
                            }
                            break;
                    }
                    break;

                case EMain.Mission:
                    switch(S3)
                    {
                        case 0:
                            await pier1Task.Run();

                            if(pier1Task.key == EHandshakeKey.Finish)
                            {
                                if(pier1Task.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMain.None, 0);
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
                            await pier2Task.Run();

                            if(pier2Task.key == EHandshakeKey.Finish)
                            {
                                if(pier2Task.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMain.None, 0);
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
                            await robotTask.Run();

                            if (robotTask.key == EHandshakeKey.Finish)
                            {
                                if(robotTask.isError)
                                {
                                    SaveState();
                                    Set(ES1.Error, EMain.None, 0);
                                }
                                else
                                {
                                    Set(EMain.HeartBeat, 0);
                                }
                            }
                            else
                            {
                                Set(EMain.HeartBeat, 0);
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
                    Set(ES1.Idle, EMain.None, 0);
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
