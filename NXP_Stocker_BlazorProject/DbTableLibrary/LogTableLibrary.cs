using System.Collections.Generic;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{

    public partial class LogTableLibrary
    {

        readonly IServiceProvider serviceProvider;

        public LogTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        //*************下面砍掉*************//

        List<LogTable> listLogTable { get; set; } = new List<LogTable>();

        //**********************************//
    }

    //**********下面先取代DB 要砍掉*************//

    public class LogTable
    {
        //public int Id { get; set; } 到時候DB要加上這個讓它自動增加

        public string LogType { get; set; }

        public string Equipment { get; set; }

        public string Msg { get; set; }

        public DateTime RecordTime { get; set; }
    }

    //***************************************//

    public partial class LogTableLibrary : ILogTableOperate
    {
        public async Task<(bool status, string msg, List<LogTable> list)> AddLogData(LogTable data)
        {
            try
            {
                listLogTable.Add(data);

                List<LogTable> list = listLogTable.Where(x => x.Equipment == data.Equipment).ToList();

                return (true, string.Empty, list);

            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
