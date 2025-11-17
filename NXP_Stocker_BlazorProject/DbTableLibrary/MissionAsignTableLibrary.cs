using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{
    //**********下面先取代DB 要砍掉*************//

    public class MissionAsignTable_stub
    {
        public string PierName { get; set; } //int

        public Guid Id { get; set; }

        public int ActionCode { get; set; }

        public string Barcode { get; set; }

        public int BoardSize { get; set; }

        public int PickZone { get; set; }

        public int PickLayer { get; set; }

        public int DropZone { get; set; }

        public int DropLayer { get; set; }

        public DateTime EstablishTime { get; set; }

        public bool IsStart { get; set; } //砍

        public DateTime StartTime { get; set; }

        public bool IsError { get; set; }

        public int ErrorCode { get; set; }

        public bool IsFinish { get; set; } //砍

        public DateTime FinishTime { get; set; }

        public bool IsCancel { get; set; }

        public virtual ICollection<MissionBase_stub> ListMission { get; set; } = new List<MissionBase_stub>();
    }

    public class MissionBase_stub
    {
        public string PierName { get; set; } //int

        public Guid Id { get; set; }

        public Guid AsignId { get; set; }

        public string Barcode { get; set; }

        public DateTime EstablishTime { get; set; }

        public bool IsStart { get; set; } //砍

        public DateTime StartTime { get; set; }

        public string Status { get; set; } //int

        public bool IsError { get; set; } 

        public int ErrorCode { get; set; }

        public bool IsFinish { get; set; } //砍

        public DateTime FinishTime { get; set; }

        public virtual MissionAsignTable_stub Asign { get; set; }
    }

    public class PierMissionTable_stub : MissionBase_stub
    {
        public int ActionCode { get; set; }
    }

    public class RobotMissionTable_stub : MissionBase_stub
    {
        public int BoardSize { get; set; }

        public int PickZone { get; set; }

        public int PickLayer { get; set; }

        public int DropZone { get; set; }

        public int DropLayer { get; set; }
    }

    //***************************************//

    public partial class MissionAsignTableLibrary
    {

        readonly IServiceProvider serviceProvider;

        public MissionAsignTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        //*************下面砍掉*************//

        List<MissionAsignTable_stub> listMissionAsignTable { get; set; } = new List<MissionAsignTable_stub>();

        //**********************************//
    }

    public partial class MissionAsignTableLibrary : IMissionAsignTableOperate
    {
        public async Task<(bool status, string msg, MissionAsignTable_stub table)> AddMissionAsign(MissionAsignTable_stub data)
        {
            try
            {
                listMissionAsignTable.Add(data);

                MissionAsignTable_stub table = listMissionAsignTable.FirstOrDefault(x => x.PierName == data.PierName
                                                                               && x.Id == data.Id
                                                                               && x.Barcode == data.Barcode);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, MissionAsignTable_stub table)> GetNewMissionAsign(string PierName)
        {
            try
            {
                MissionAsignTable_stub table = listMissionAsignTable.FirstOrDefault(x => x.PierName == PierName
                                                                               && x.IsStart == false
                                                                               && x.IsFinish == false);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, MissionAsignTable_stub table)> UpdateMissionAsign(MissionAsignTable_stub data)
        {
            try
            {
                int index = listMissionAsignTable.FindIndex(x => x.PierName == data.PierName
                                                              && x.Id == data.Id
                                                              && x.ActionCode == data.ActionCode
                                                              && x.Barcode == data.Barcode
                                                              && x.BoardSize == data.BoardSize
                                                              && x.PickZone == data.PickZone
                                                              && x.PickLayer == data.PickLayer
                                                              && x.DropZone == data.DropZone
                                                              && x.DropLayer == data.DropLayer
                                                              && x.EstablishTime == data.EstablishTime);

                listMissionAsignTable[index].IsStart = data.IsStart;
                listMissionAsignTable[index].StartTime = data.StartTime;
                listMissionAsignTable[index].IsError = data.IsError;
                listMissionAsignTable[index].ErrorCode = data.ErrorCode;
                listMissionAsignTable[index].IsFinish = data.IsFinish;
                listMissionAsignTable[index].FinishTime = data.FinishTime;

                return (true, string.Empty, listMissionAsignTable[index]);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
