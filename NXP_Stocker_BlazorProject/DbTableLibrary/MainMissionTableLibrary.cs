using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{
    public partial class MainMissionTableLibrary
    {

        readonly IServiceProvider serviceProvider;

        public MainMissionTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        //*************下面砍掉*************//

        List<MainMissionTable> listMainMissionTable { get; set; } = new List<MainMissionTable>();

        //**********************************//
    }

    //**********下面先取代DB 要砍掉*************//

    public class MainMissionTable
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

    public partial class MainMissionTableLibrary : IMainMissionTableOperate
    {
        public async Task<(bool status, string msg, MainMissionTable table)> AddMainMissionTable(MainMissionTable data)
        {
            try
            {
                listMainMissionTable.Add(data);

                MainMissionTable table = listMainMissionTable.FirstOrDefault(x => x.PierName == data.PierName
                                                                               && x.MissionSerialNumber == data.MissionSerialNumber
                                                                               && x.Barcode == data.Barcode);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
