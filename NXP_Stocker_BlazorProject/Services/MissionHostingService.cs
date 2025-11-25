
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.Services.Interface;

namespace NXP_Stocker_BlazorProject.Services
{
    public class MissionHostingService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        public MissionHostingService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var machineService = scope.ServiceProvider.GetRequiredService<IMachineService>();
                await machineService.Initial();
            }
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = scopeFactory.CreateScope())
                {
                    var missionTableLibrary = scope.ServiceProvider.GetRequiredService<IMissionTableOperate>();
                    await missionTableLibrary.RemoveFinishedMission();
                    await Task.Delay(3000, stoppingToken);

                }
            }

        }
    }
}
