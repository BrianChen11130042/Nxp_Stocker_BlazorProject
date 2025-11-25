using Microsoft.EntityFrameworkCore;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;
using NXP_Stocker_BlazorProject.Scope;
using NXP_Stocker_BlazorProject.Services;
using NXP_Stocker_BlazorProject.Services.Interface;

namespace NXP_Stocker_BlazorProject
{
    public static class NxpServiceBExtension
    {
        public static IHostApplicationBuilder AddNxpServiceB(this IHostApplicationBuilder builder, string dbConnectionStringName = "NXPStorageConnectionString")
        {
            #region DB Table
            builder.Services.AddDbContext<NxpMachineDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString(dbConnectionStringName));
            });
            builder.Services.AddSingleton<ILogTableOperate, LogTableLibrary>();
            builder.Services.AddSingleton<IMissionTableOperate, MissionTableLibrary>();

            #endregion

            #region CommonService

            builder.Services.AddSingleton<ObserverService>();

            #endregion

            #region Scope

            builder.Services.AddSingleton<MachineScope>();

            #endregion

            #region Service

            builder.Services.AddSingleton<IMachineService, MachineService>();
            builder.Services.AddHostedService<MissionHostingService>();

            #endregion

            return builder;
        }
    }
}
