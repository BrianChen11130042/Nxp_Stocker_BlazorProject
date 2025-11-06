using CommonLibraryB.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class MissionAsignTask : IMissionAsignTaskPack
    {
        readonly IMissionAsignTaskPack pack;

        public MissionAsignTask(IMissionAsignTaskPack pack)
        {
            this.pack = pack;
            interval = 10;
        }

        public Task<bool> GetTableWarehousePickPort()
        {
            return pack.GetTableWarehousePickPort();
        }

        public Task<bool> GetPlcPierName()
        {
            return pack.GetPlcPierName();
        }

        public Task<bool> GetTableNewMissionAsign()
        {
            return pack.GetTableNewMissionAsign();
        }

        public bool IsInputWarehouse()
        {
            return pack.IsInputWarehouse();
        }

        public bool IsOutputWarehouse()
        {
            return pack.IsOutputWarehouse();
        }

        public bool IsTransformWarehouse()
        {
            return pack.IsTransformWarehouse();
        }

        public Task UpdateUIMissionAsign()
        {
            return pack.UpdateUIMissionAsign();
        }

        public Task<bool> GetTableWarehouseDropPort()
        {
            return pack.GetTableWarehouseDropPort();
        }

        public Task<bool> SetTableNewPierMission()
        {
            return pack.SetTableNewPierMission();
        }

        public Task<bool> SetTableMissionAsignStart()
        {
            return pack.SetTableMissionAsignStart();
        }
    }

    public enum EMissionAssign
    {
        None,
        CheckMission,
        InputWarehouse,
        OutputWarehouse,
        TransformWarehouse
    }

    public partial class MissionAsignTask : FSMBase<EMissionAssign, int>
    {
        public async override Task Action()
        {
            key = EHandshakeKey.Run;

            switch(S2)
            {
                case EMissionAssign.None:
                    Set(ES1.Finish, EMissionAssign.None, 0);
                    break;

                case EMissionAssign.CheckMission:
                    switch(S3)
                    {
                        case 0:
                            if(await GetPlcPierName())
                            {
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 10:
                            if(await GetTableNewMissionAsign())
                            {
                                await UpdateUIMissionAsign();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 20:
                            if(IsInputWarehouse())
                            {
                                Set(EMissionAssign.InputWarehouse, 0);
                            }
                            else if(IsOutputWarehouse())
                            {
                                Set(EMissionAssign.OutputWarehouse, 0);
                            }
                            else if(IsTransformWarehouse())
                            {
                                Set(EMissionAssign.TransformWarehouse, 0);
                            }
                            else
                            {
                                Set(0);
                            }
                            break;
                    }
                    break;

                case EMissionAssign.InputWarehouse:
                    switch(S3)
                    {
                        case 0:
                            if(await GetTableWarehousePickPort())
                            {
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 10:
                            if(await GetTableWarehouseDropPort())
                            {
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 20:
                            if(await SetTableNewPierMission())
                            {
                                Set(30);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 30:
                            if(await SetTableMissionAsignStart())
                            {

                            }
                            else
                            {

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
            Set(ES1.Idle, EMissionAssign.None, 0);
        }

        public async override Task Finish()
        {
            isError = false;
            key = EHandshakeKey.Finish;
            Set(ES1.Idle, EMissionAssign.None, 0);
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
