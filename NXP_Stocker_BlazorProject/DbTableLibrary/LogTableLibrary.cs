using System.Collections.Generic;
using CommonLibraryP.MapPKG;
using Microsoft.EntityFrameworkCore;
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
    }

    public partial class LogTableLibrary : ILogTableOperate
    {
        public async Task<(bool status, string msg, List<LogTable> list)> AddLogData(LogTable data)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

                    context.LogTables.Add(data);

                    await context.SaveChangesAsync();

                    DateTime timePoint = DateTime.Now.AddDays(-2);

                    List<LogTable> list = await context.LogTables.AsNoTracking()
                                                                 .Where(x => x.RecordTime != null
                                                                          && x.RecordTime >= timePoint
                                                                          && x.Equipment == data.Equipment)
                                                                 .OrderByDescending(x => x.RecordTime)
                                                                 .Take(100)
                                                                 .ToListAsync();

                    return (true, string.Empty, list);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
