using Microsoft.EntityFrameworkCore;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;
using System.ComponentModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{

    public partial class MissionTableLibrary : IMissionTableOperate
    {

        readonly IServiceProvider serviceProvider;

        readonly object _missionAsignLock = new object();

        public MissionTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public event Func<Task>? MissionAssignInQueueChangedAct;

        List<MissionAssignTable> MissionAssignInQueue { get; set; } = new List<MissionAssignTable>();
    }

    public partial class MissionTableLibrary
    {
        public async Task<(bool status, string msg)> InitMissionAsignToInQue()
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

                    List<MissionAssignTable> list = await context.MissionAssignTables.Include(x => x.Missions)
                                                                                   .AsNoTracking()
                                                                                   .Where(x => x.FinishTime != null)
                                                                                   .OrderBy(x => x.EstablishTime)
                                                                                   .ToListAsync();

                    list = list.Where(x => x.IsFinish == false).ToList();

                    lock(_missionAsignLock)
                    {
                        MissionAssignInQueue.Clear();
                        MissionAssignInQueue = list;
                    }

                    MissionAssignInQueueChangedAct?.Invoke();
                    return (true, string.Empty);
                }
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public Task RemoveFinishedMission()
        {
            lock(_missionAsignLock)
            {
                MissionAssignInQueue.RemoveAll(x => x.IsFinish == true);
            }

            MissionAssignInQueueChangedAct?.Invoke();
            return Task.CompletedTask;
        }

        public async Task<List<MissionAssignTable>> GetMissionAssignFromInQueue()
        {
            lock(_missionAsignLock)
            {
                return MissionAssignInQueue.OrderBy(x => x.EstablishTime).ToList();
            }
        }

        public async Task<(bool status, string msg, MissionAssignTable table)> UpSertMissionAsign(MissionAssignTable data)
        {
            try
            {
                MissionAssignTable table;

                using (var scope = serviceProvider.CreateScope())
                {
                    NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

                    var target = await context.MissionAssignTables.FirstOrDefaultAsync(x => x.Id == data.Id);

                    if (target != null)
                    {
                        context.Entry(target).CurrentValues.SetValues(data);
                        table = target;
                    }
                    else
                    {
                        context.MissionAssignTables.Add(data);
                        table = data;
                    }

                    await context.SaveChangesAsync();

                    await UpsertMissionAsignToInQueue(data);

                    return (true, string.Empty, table);

                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        async Task UpsertMissionAsignToInQueue(MissionAssignTable data)
        {
            bool notify = false;

            lock(_missionAsignLock)
            {
                var target = MissionAssignInQueue.FirstOrDefault(x => x.Id == data.Id);

                if (target != null)
                {
                    target.StartTime = data.StartTime;
                    target.ErrorCode = data.ErrorCode;
                    target.FinishTime = data.FinishTime;
                    target.IsCancel = data.IsCancel;
                }
                else
                {
                    MissionAssignInQueue.Add(data);
                    notify = true;
                }
            }

            if(notify)
                MissionAssignInQueueChangedAct?.Invoke();
        }

        public async Task<(bool status, string msg, MissionAssignTable table)> GetNewMissionAsign(bool IsStart,
                                                                                                 bool IsFinish,
                                                                                                 int PierNo)
        {

            MissionAssignTable table;

            lock(_missionAsignLock)
            {
                table = MissionAssignInQueue.Where(x => x.PierNo == PierNo
                                                     && x.IsStart == IsStart
                                                     && x.IsFinish == IsFinish)
                                            .OrderBy(x => x.EstablishTime)
                                            .FirstOrDefault();
            }

            return (true, string.Empty, table);

            //try
            //{
            //    using (var scope = serviceProvider.CreateScope())
            //    {
            //        NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

            //        var tables = await context.MissionAsignTables.Include(x => x.Missions)
            //                                                             .AsNoTracking()
            //                                                             .Where(x => x.PierNo == PierNo)
            //                                                             .OrderBy(x => x.EstablishTime)
            //                                                             .ToListAsync();

            //        var table = tables.FirstOrDefault(x => x.IsStart == IsStart && x.IsFinish == IsFinish);

            //        return (true, string.Empty, table);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return (false, ex.Message, null);
            //}
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

                var table = list.OrderBy(x => x.EstablishTime)
                                .OfType<T>()
                                .FirstOrDefault();

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, T table)> GetMissionById<T>(Guid Id) where T : MissionBase
        {
            lock(_missionAsignLock)
            {
                foreach (var missionAsign in MissionAssignInQueue)
                {
                    var item = missionAsign.Missions.FirstOrDefault(x => x.Id == Id);

                    if (item != null && item is T table)
                    {
                        return (true, string.Empty, table);
                    }
                }
            }

            return (false, "Not Found Mission Data", null);


            //try
            //{
            //    using (var scope = serviceProvider.CreateScope())
            //    {
            //        NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

            //        MissionBase missionBase = await context.MissionBases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);

            //        if(missionBase != null && missionBase is T table)
            //        {
            //            return (true, string.Empty, table);
            //        }
            //        else
            //        {
            //            return (false, "Not Found Mission Data", null);
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{
            //    return (false, ex.Message, null);
            //}
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

                    await _UpsertMissionToInQue<T>(data);

                    return (true, string.Empty, table);
                }
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        async Task _UpsertMissionToInQue<T>(T data) where T : MissionBase
        {
            bool notify = false;

            lock (_missionAsignLock)
            {
                var missionAsign = MissionAssignInQueue.FirstOrDefault(x => x.Id == data.AsignId);

                if (missionAsign != null)
                {
                    var target = missionAsign.Missions.FirstOrDefault(x => x.Id == data.Id);

                    if (target != null)
                    {
                        target.StartTime = data.StartTime;
                        target.Status = data.Status;
                        target.ErrorCode = data.ErrorCode;
                        target.FinishTime = data.FinishTime;
                    }
                    else
                    {
                        missionAsign.Missions.Add(data);
                    }
                    notify = true;
                }
            }

            if(notify)
                MissionAssignInQueueChangedAct?.Invoke();
        }

        public async Task UpdateMissionStatusToInQue<T>(T data) where T : MissionBase
        {
            bool notify = false;

            lock (_missionAsignLock)
            {
                var missionAsign = MissionAssignInQueue.FirstOrDefault(x => x.Id == data.AsignId);

                if (missionAsign != null)
                {
                    var target = missionAsign.Missions.FirstOrDefault(x => x.Id == data.Id);

                    if (target != null)
                    {
                        target.Status = data.Status;
                        notify = true;
                    }
                }
            }

            if(notify)
                MissionAssignInQueueChangedAct?.Invoke();
        }
    }

    public partial class MissionTableLibrary
    {
        async Task<List<MissionBase>> _getMissionBaseByParam(int PierNo, bool IsStart, bool IsFinish)
        {
            List<MissionBase> listResult = new List<MissionBase>();

            lock (_missionAsignLock)
            {
                foreach (var missionAsign in MissionAssignInQueue)
                {
                    List<MissionBase> listMissionBase = missionAsign.Missions.Where(x => x.PierNo == PierNo
                                                                                      && x.IsStart == IsStart
                                                                                      && x.IsFinish == IsFinish)
                                                                             .ToList();

                    listResult.AddRange(listMissionBase);
                }
            }

            listResult = listResult.OrderBy(x => x.EstablishTime).ToList();

            return listResult;

            //using (var scope = serviceProvider.CreateScope())
            //{
            //    NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

            //    List<MissionBase> list = await context.MissionBases.AsNoTracking().Where(x => x.PierNo == PierNo).ToListAsync();

            //    list = list.Where(x=> x.IsStart == IsStart && x.IsFinish == IsFinish).OrderBy(x => x.EstablishTime).ToList();

            //    return list;
            //}
        }

        async Task<List<MissionBase>> _getMissionBaseByParam(bool IsStart, bool IsFinish)
        {
            List<MissionBase> listResult = new List<MissionBase>();

            lock (_missionAsignLock)
            {
                foreach (var missionAsign in MissionAssignInQueue)
                {
                    List<MissionBase> listMissionBase = missionAsign.Missions.Where(x => x.IsStart == IsStart
                                                                                      && x.IsFinish == IsFinish)
                                                                             .ToList();

                    listResult.AddRange(listMissionBase);
                }
            }

            listResult = listResult.OrderBy(x => x.EstablishTime).ToList();

            return listResult;


            //using (var scope = serviceProvider.CreateScope())
            //{
            //    NxpMachineDbContext context = scope.ServiceProvider.GetRequiredService<NxpMachineDbContext>();

            //    List<MissionBase> list = await context.MissionBases.AsNoTracking().ToListAsync();

            //    list = list.Where(x => x.IsStart == IsStart && x.IsFinish == IsFinish).OrderBy(x => x.EstablishTime).ToList();

            //    return list;
            //}
        }
    }
}
