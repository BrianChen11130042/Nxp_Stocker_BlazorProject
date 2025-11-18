using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{
    public partial class RobotMissionTableLibrary
    {
        readonly IServiceProvider serviceProvider;

        public RobotMissionTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        //*************下面砍掉*************//

        List<RobotMissionTable_stub> listRobotMissionTable { get; set; } = new List<RobotMissionTable_stub>();

        //**********************************//
    }

    public partial class RobotMissionTableLibrary : IRobotMissionTableOperate
    {
        public async Task<(bool status, string msg, RobotMissionTable_stub table)> AddRobotMission(RobotMissionTable_stub data)
        {
            try
            {
                listRobotMissionTable.Add(data);

                RobotMissionTable_stub table = listRobotMissionTable.FirstOrDefault(x => x.PierName == data.PierName 
                                                                                 && x.AsignId == data.AsignId
                                                                                 && x.Barcode == data.Barcode
                                                                                 && x.BoardSize == data.BoardSize
                                                                                 && x.PickZone == data.PickZone
                                                                                 && x.PickLayer == data.PickLayer
                                                                                 && x.DropZone == data.DropZone
                                                                                 && x.DropLayer == data.DropLayer
                                                                                 && x.EstablishTime == data.EstablishTime);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, RobotMissionTable_stub table)> GetNewRobotMission()
        {
            try
            {
                RobotMissionTable_stub table = listRobotMissionTable.FirstOrDefault(x => x.IsStart == false
                                                                                 && x.IsFinish == false);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, RobotMissionTable_stub table)> UpdateRobotMission(RobotMissionTable_stub data)
        {
            try
            {
                int index = listRobotMissionTable.FindIndex(x => x.PierName == data.PierName
                                                              && x.AsignId == data.AsignId
                                                              && x.Barcode == data.Barcode
                                                              && x.BoardSize == data.BoardSize
                                                              && x.PickZone == data.PickZone
                                                              && x.PickLayer == data.PickLayer
                                                              && x.DropZone == data.DropZone
                                                              && x.DropLayer == data.DropLayer
                                                              && x.EstablishTime == data.EstablishTime);

                listRobotMissionTable[index].IsStart = data.IsStart;
                listRobotMissionTable[index].StartTime = data.StartTime;
                listRobotMissionTable[index].IsError = data.IsError;
                listRobotMissionTable[index].ErrorCode = data.ErrorCode;
                listRobotMissionTable[index].IsFinish = data.IsFinish;
                listRobotMissionTable[index].FinishTime = data.FinishTime;

                return (true, string.Empty, listRobotMissionTable[index]);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, RobotMissionTable_stub table)> GetTargetRobotMission(RobotMissionTable_stub data)
        {
            try
            {
                int index = listRobotMissionTable.FindIndex(x => x.PierName == data.PierName
                                                              && x.AsignId == data.AsignId
                                                              && x.Barcode == data.Barcode
                                                              && x.BoardSize == data.BoardSize
                                                              && x.PickZone == data.PickZone
                                                              && x.PickLayer == data.PickLayer
                                                              && x.DropZone == data.DropZone
                                                              && x.DropLayer == data.DropLayer
                                                              && x.EstablishTime == data.EstablishTime);

                return (true, string.Empty, listRobotMissionTable[index]);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }

}
