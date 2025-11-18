using CommonLibraryB_NXP.Base.FiniteStateMachine;
using NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.Tasks
{
    public partial class MissionAsignTask : IMissionAsignTaskPack
    {
        readonly IMissionAsignTaskPack pack;

        public MissionAsignTask(IMissionAsignTaskPack pack)
        {
            this.pack = pack;
            interval = 1;
        }

        public Task<bool> GetTableWarehousePickPort()
        {
            return pack.GetTableWarehousePickPort();
        }

        public Task<bool> GetPlcPierNo()
        {
            return pack.GetPlcPierNo();
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

        public Task<bool> SetLogMissionAsignStart()
        {
            return pack.SetLogMissionAsignStart();
        }

        public Task UpdateUIMissionAsignLog()
        {
            return pack.UpdateUIMissionAsignLog();
        }

        public Task<bool> GetTablePierMissionStatus()
        {
            return pack.GetTablePierMissionStatus();
        }

        public bool IsPierMissionFinish()
        {
            return pack.IsPierMissionFinish();
        }

        public Task<bool> SetTableWarehouseInputPickPort()
        {
            return pack.SetTableWarehouseInputPickPort();
        }

        public Task<bool> SetTableNewRobotMission()
        {
            return pack.SetTableNewRobotMission();
        }

        public Task<bool> GetTableRobotMissionStatus()
        {
            return pack.GetTableRobotMissionStatus();
        }

        public bool IsRobotMissionError()
        {
            return pack.IsRobotMissionError();
        }

        public bool IsRobotMissionFinish()
        {
            return pack.IsRobotMissionFinish();
        }

        public Task<bool> SetTableWarehouseOutputPickPort()
        {
            return pack.SetTableWarehouseOutputPickPort();
        }

        public Task<bool> SetTableWarehouseInputDropPort()
        {
            return pack.SetTableWarehouseInputDropPort();
        }

        public Task<bool> SetTableMissionAsignFinsih()
        {
            return pack.SetTableMissionAsignFinsih();
        }

        public Task<bool> SetLogMissionAsignFinish()
        {
            return pack.SetLogMissionAsignFinish();
        }

        public Task<bool> SetTableWarehouseOutputDropPort()
        {
            return pack.SetTableWarehouseOutputDropPort();
        }

        public Task<bool> GetPlcIsReady()
        {
            return pack.GetPlcIsReady();
        }

        public bool IsPlcReady()
        {
            return pack.IsPlcReady();
        }

        public Task UpdateUIPickPortWarehouse()
        {
            return pack.UpdateUIPickPortWarehouse();
        }

        public Task UpdateUIDropPortWarehouse()
        {
            return pack.UpdateUIDropPortWarehouse();
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
                            if(await GetPlcPierNo())
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
                            if(await GetPlcIsReady())
                            {
                                if(IsPlcReady())
                                {
                                    Set(20);
                                }
                                else
                                {
                                    Set(0);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 20:
                            if(await GetTableNewMissionAsign())
                            {
                                await UpdateUIMissionAsign();
                                Set(30);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 30:
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
                                await UpdateUIPickPortWarehouse();
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
                                await UpdateUIDropPortWarehouse();
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
                                await UpdateUIMissionAsign();
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 40:
                            if(await SetLogMissionAsignStart())
                            {
                                await UpdateUIMissionAsignLog();
                                Set(50);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 50:
                            if(await GetTablePierMissionStatus())
                            {
                                if (IsPierMissionFinish())
                                {
                                    Set(60);
                                }
                                else
                                {
                                    Set(50);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 60:
                            if(await SetTableWarehouseInputPickPort())
                            {
                                await UpdateUIPickPortWarehouse();
                                Set(70);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 70:
                            if(await SetTableNewRobotMission())
                            {
                                Set(80);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 80:
                            if(await GetTableRobotMissionStatus())
                            {
                                if(IsRobotMissionFinish())
                                {
                                    if(IsRobotMissionError())
                                    {
                                        //後續跟電控討論
                                    }
                                    else
                                    {
                                        Set(90);
                                    }
                                }
                                else
                                {
                                    Set(80);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 90:
                            if(await SetTableWarehouseOutputPickPort())
                            {
                                await UpdateUIPickPortWarehouse();
                                Set(100);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 100:
                            if(await SetTableWarehouseInputDropPort())
                            {
                                await UpdateUIDropPortWarehouse();
                                Set(110);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 110:
                            if(await SetTableMissionAsignFinsih())
                            {
                                await UpdateUIMissionAsign();
                                Set(120);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 120:
                            if(await SetLogMissionAsignFinish())
                            {
                                await UpdateUIMissionAsignLog();
                                Set(EMissionAssign.CheckMission, 0);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;
                    }
                    break;

                case EMissionAssign.OutputWarehouse:
                    switch(S3)
                    {
                        case 0:
                            if (await GetTableWarehousePickPort())
                            {
                                await UpdateUIPickPortWarehouse();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 10:
                            if (await GetTableWarehouseDropPort())
                            {
                                await UpdateUIDropPortWarehouse();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 20:
                            if(await SetTableNewRobotMission())
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
                            if (await SetTableMissionAsignStart())
                            {
                                await UpdateUIMissionAsign();
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 40:
                            if (await SetLogMissionAsignStart())
                            {
                                await UpdateUIMissionAsignLog();
                                Set(50);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 50:
                            if(await GetTableRobotMissionStatus())
                            {
                                if (IsRobotMissionFinish())
                                {
                                    if (IsRobotMissionError())
                                    {
                                        //後續跟電控討論
                                    }
                                    else
                                    {
                                        Set(60);
                                    }
                                }
                                else
                                {
                                    Set(50);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 60:
                            if (await SetTableWarehouseOutputPickPort())
                            {
                                await UpdateUIPickPortWarehouse();
                                Set(70);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 70:
                            if (await SetTableWarehouseInputDropPort())
                            {
                                await UpdateUIDropPortWarehouse();
                                Set(80);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 80:
                            if (await SetTableNewPierMission())
                            {
                                Set(90);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 90:
                            if (await GetTablePierMissionStatus())
                            {
                                if (IsPierMissionFinish())
                                {
                                    Set(100);
                                }
                                else
                                {
                                    Set(90);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 100:
                            if(await SetTableWarehouseOutputDropPort())
                            {
                                await UpdateUIDropPortWarehouse();
                                Set(110);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 110:
                            if (await SetTableMissionAsignFinsih())
                            {
                                await UpdateUIMissionAsign();
                                Set(120);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 120:
                            if (await SetLogMissionAsignFinish())
                            {
                                await UpdateUIMissionAsignLog();
                                Set(EMissionAssign.CheckMission, 0);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;
                    }
                    break;

                case EMissionAssign.TransformWarehouse:
                    switch(S3)
                    {
                        case 0:
                            if (await GetTableWarehousePickPort())
                            {
                                await UpdateUIPickPortWarehouse();
                                Set(10);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 10:
                            if (await GetTableWarehouseDropPort())
                            {
                                await UpdateUIDropPortWarehouse();
                                Set(20);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 20:
                            if (await SetTableNewRobotMission())
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
                            if (await SetTableMissionAsignStart())
                            {
                                await UpdateUIMissionAsign();
                                Set(40);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 40:
                            if (await SetLogMissionAsignStart())
                            {
                                await UpdateUIMissionAsignLog();
                                Set(50);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 50:
                            if (await GetTableRobotMissionStatus())
                            {
                                if (IsRobotMissionFinish())
                                {
                                    if (IsRobotMissionError())
                                    {
                                        //後續跟電控討論
                                    }
                                    else
                                    {
                                        Set(60);
                                    }
                                }
                                else
                                {
                                    Set(50);
                                }
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 60:
                            if (await SetTableWarehouseOutputPickPort())
                            {
                                await UpdateUIPickPortWarehouse();
                                Set(70);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 70:
                            if (await SetTableWarehouseInputDropPort())
                            {
                                await UpdateUIDropPortWarehouse();
                                Set(80);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 80:
                            if (await SetTableMissionAsignFinsih())
                            {
                                await UpdateUIMissionAsign();
                                Set(90);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
                            }
                            break;

                        case 90:
                            if (await SetLogMissionAsignFinish())
                            {
                                await UpdateUIMissionAsignLog();
                                Set(EMissionAssign.CheckMission, 0);
                            }
                            else
                            {
                                SaveState();
                                Set(ES1.Error, EMissionAssign.None, 0);
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
