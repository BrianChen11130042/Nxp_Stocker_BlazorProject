using DevExpress.ClipboardSource.SpreadsheetML;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{
    public partial class PierMissionTableLibrary
    {
        readonly IServiceProvider serviceProvider;

        public PierMissionTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        //*************下面砍掉*************//

        List<PierMissionTable> listPierMissionTable { get; set; } = new List<PierMissionTable>();

        //**********************************//
    }

    //**********下面先取代DB 要砍掉*************//

    public class PierMissionTable
    {
        //public int Id { get; set; } 到時候DB要加上這個讓它自動增加

        public string PierName { get; set; }

        public string MissionSerialNumber { get; set; }

        public string Barcode { get; set; }

        public int ActionCode { get; set; }

        public DateTime EstablishTime { get; set; }

        public bool IsStart { get; set; }

        public DateTime StartTime { get; set; }

        public string StepStatus { get; set; } //只是給UI看的狀態, 表示現在Pier動作做到哪(不用在資料庫中操作)

        public bool IsError { get; set; }

        public int ErrorCode { get; set; }

        public bool IsFinish { get; set; }

        public DateTime FinishTime { get; set; }
    }

    //***************************************//

    public partial class PierMissionTableLibrary : IPierMissionTableOperate
    {
        public async Task<(bool status, string msg, PierMissionTable table)> AddPierMission(PierMissionTable data)
        {
            try
            {
                listPierMissionTable.Add(data);

                PierMissionTable table = listPierMissionTable.FirstOrDefault(x => x.PierName == data.PierName
                                                                               && x.MissionSerialNumber == data.MissionSerialNumber
                                                                               && x.Barcode == data.Barcode);

                return (true, string.Empty, table);

            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, PierMissionTable table)> GetNewPierMission(string PierName)
        {
            try
            {
                PierMissionTable table = listPierMissionTable.FirstOrDefault(x => x.PierName == PierName
                                                                               && x.IsStart == false
                                                                               && x.IsFinish == false);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, PierMissionTable table)> UpdatePierMission(PierMissionTable data)
        {
            try
            {
                int index = listPierMissionTable.FindIndex(x => x.PierName == data.PierName
                                                             && x.MissionSerialNumber == data.MissionSerialNumber
                                                             && x.Barcode == data.Barcode
                                                             && x.ActionCode == data.ActionCode
                                                             && x.EstablishTime == data.EstablishTime);

                listPierMissionTable[index].IsStart = data.IsStart;
                listPierMissionTable[index].StartTime = data.StartTime;
                listPierMissionTable[index].IsError = data.IsError;
                listPierMissionTable[index].ErrorCode = data.ErrorCode;
                listPierMissionTable[index].IsFinish = data.IsFinish;
                listPierMissionTable[index].FinishTime = data.FinishTime;

                return (true, string.Empty, listPierMissionTable[index]);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
