using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{
    public partial class MissionAsignTableLibrary
    {

        readonly IServiceProvider serviceProvider;

        public MissionAsignTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        //*************下面砍掉*************//

        List<MissionAsignTable> listMissionAsignTable { get; set; } = new List<MissionAsignTable>();

        //**********************************//
    }

    //**********下面先取代DB 要砍掉*************//

    public class MissionAsignTable
    {
        //public int Id { get; set; } 到時候DB要加上這個讓它自動增加

        public string PierName { get; set; }

        public string MissionSerialNumber { get; set; }

        public int ActionCode { get; set; }

        public string Barcode { get; set; }

        public int BoardSize { get; set; }

        public int PickZone { get; set; }

        public int PickLayer { get; set; }

        public int DropZone { get; set; }

        public int DropLayer { get; set; }

        public DateTime EstablishTime { get; set; }

        public bool IsStart { get; set; }

        public DateTime StartTime { get; set; }

        public bool IsError { get; set; }

        public int ErrorCode { get; set; }

        public bool IsFinish { get; set; }

        public DateTime FinishTime { get; set; }
    }

    //***************************************//

    public partial class MissionAsignTableLibrary : IMissionAsignTableOperate
    {
        public async Task<(bool status, string msg, MissionAsignTable table)> AddMissionAsign(MissionAsignTable data)
        {
            try
            {
                listMissionAsignTable.Add(data);

                MissionAsignTable table = listMissionAsignTable.FirstOrDefault(x => x.PierName == data.PierName
                                                                               && x.MissionSerialNumber == data.MissionSerialNumber
                                                                               && x.Barcode == data.Barcode);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, MissionAsignTable table)> GetNewMissionAsign(string PierName)
        {
            try
            {
                MissionAsignTable table = listMissionAsignTable.FirstOrDefault(x => x.PierName == PierName
                                                                               && x.IsStart == false
                                                                               && x.IsFinish == false);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, MissionAsignTable table)> UpdateMissionAsign(MissionAsignTable data)
        {
            try
            {
                int index = listMissionAsignTable.FindIndex(x => x.PierName == data.PierName
                                                              && x.MissionSerialNumber == data.MissionSerialNumber
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
