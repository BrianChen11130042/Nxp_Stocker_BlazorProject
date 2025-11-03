using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

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

        List<RobotMissionTable> listRobotMissionTable { get; set; } = new List<RobotMissionTable>();

        //**********************************//
    }

    //**********下面先取代DB 要砍掉*************//

    public class RobotMissionTable
    {
        //public int Id { get; set; } 到時候DB要加上這個讓它自動增加

        public string PierName { get; set; }

        public string MissionSerialNumber { get; set; }

        public string Barcode { get; set; }

        public int BoardSize { get; set; }

        public int PickZone { get; set; }

        public int PickLayer { get; set; }

        public int DropZone { get; set; }

        public int DropLayer { get; set; }

        public DateTime EstablishTime { get; set; }

        public bool IsStart { get; set; }

        public DateTime StartTime { get; set; }

        public string Status { get; set; }

        public bool IsError { get; set; }

        public int ErrorCode { get; set; }

        bool IsFinish { get; set; }

        public DateTime FinishTime { get; set; }
    }

    //***************************************//

    public partial class RobotMissionTableLibrary : IRobotMissionTableOperate
    {
        public async Task<(bool status, string msg, RobotMissionTable table)> AddRobotMission(RobotMissionTable data)
        {
            try
            {
                listRobotMissionTable.Add(data);

                RobotMissionTable table = listRobotMissionTable.FirstOrDefault(x => x.PierName == data.PierName 
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
