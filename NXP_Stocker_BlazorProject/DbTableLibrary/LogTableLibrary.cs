using System.Collections.Generic;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

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
