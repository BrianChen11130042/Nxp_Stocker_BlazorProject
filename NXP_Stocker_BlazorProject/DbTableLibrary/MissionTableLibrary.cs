using Microsoft.EntityFrameworkCore;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;
using System.ComponentModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{

    public partial class MissionTableLibrary
    {

        readonly IServiceProvider serviceProvider;

        public MissionTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }

    public partial class MissionTableLibrary : IMissionTableOperate
    {
        public async Task<(bool status, string msg, MissionAsignTable table)> GetNewMissionAsign(bool IsStart, 
                                                                                                 bool IsFinish, 
                                                                                                 int PierNo)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

                    var tables = await context.MissionAsignTables.Include(x => x.Missions)
                                                                         .AsNoTracking()
                                                                         .Where(x => x.PierNo == PierNo)
                                                                         .OrderBy(x => x.EstablishTime)
                                                                         .ToListAsync();

                    var table = tables.FirstOrDefault(x => x.IsStart == IsStart && x.IsFinish == IsFinish);

                    return (true, string.Empty, table);
                }
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, MissionAsignTable table)> UpSertMissionAsign(MissionAsignTable data)
        {
            try
            {
                MissionAsignTable table;

                using (var scope = serviceProvider.CreateScope())
                {
                    NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

                    var target = await context.MissionAsignTables.FirstOrDefaultAsync(x => x.Id == data.Id);

                    if(target != null)
                    {
                        context.Entry(target).CurrentValues.SetValues(data);
                        table = target;
                    }
                    else
                    {
                        context.MissionAsignTables.Add(data);
                        table = data;
                    }

                    await context.SaveChangesAsync();

                    return (true, string.Empty, table);

                }
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }

    public partial class MissionTableLibrary
    {
        public async Task<(bool status, string msg, T table)> GetNewMission<T>(bool IsStart, 
                                                                               bool IsFinish, 
                                                                               int PierNo) where T : MissionBase
        {
            try
            {
                List<MissionBase> list = new List<MissionBase>();

                if (PierNo == 0)
                    list = await _getMissionBaseByParam(IsStart, IsFinish);
                else
                    list = await _getMissionBaseByParam(PierNo, IsStart, IsFinish);

                foreach (var item in list)
                {
                    if (item is T table)
                    {
                        return (true, string.Empty, table);
                    }
                }

                return (true, string.Empty, null);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, T table)> GetMissionById<T>(Guid Id) where T : MissionBase
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

                    MissionBase missionBase = await context.MissionBases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);

                    if(missionBase != null && missionBase is T table)
                    {
                        return (true, string.Empty, table);
                    }
                    else
                    {
                        return (false, "Not Found Mission Data", null);
                    }
                }
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, T table)> UpSertMission<T>(T data) where T : MissionBase
        {
            try
            {
                T table;

                using (var scope = serviceProvider.CreateScope())
                {
                    NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

                    var target = await context.Set<T>().FirstOrDefaultAsync(x => x.Id == data.Id);

                    if(target != null)
                    {
                        context.Entry<T>(target).CurrentValues.SetValues(data);
                        table = target;
                    }
                    else
                    {
                        context.Set<T>().Add(data);
                        table = data;
                    }

                    await context.SaveChangesAsync();

                    return (true, string.Empty, table);
                }
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }

    public partial class MissionTableLibrary
    {
        async Task<List<MissionBase>> _getMissionBaseByParam(int PierNo, bool IsStart, bool IsFinish)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

                List<MissionBase> list = await context.MissionBases.AsNoTracking().Where(x => x.PierNo == PierNo).ToListAsync();

                list = list.Where(x=> x.IsStart == IsStart && x.IsFinish == IsFinish).OrderBy(x => x.EstablishTime).ToList();

                return list;
            }
        }

        async Task<List<MissionBase>> _getMissionBaseByParam(bool IsStart, bool IsFinish)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

                List<MissionBase> list = await context.MissionBases.AsNoTracking().ToListAsync();

                list = list.Where(x => x.IsStart == IsStart && x.IsFinish == IsFinish).OrderBy(x => x.EstablishTime).ToList();

                return list;
            }
        }
    }
}
