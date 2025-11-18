using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.Scope;
using NXP_Stocker_BlazorProject.Services.Interface;
using NXP_Stocker_BlazorProject.Services;

namespace NXP_Stocker_BlazorProject
{
    public static class NxpServiceBExtension
    {
        public static IHostApplicationBuilder AddNxpServiceB(this IHostApplicationBuilder builder)
        {
            #region DB Table

            builder.Services.AddSingleton<ILogTableOperate, LogTableLibrary>();
            builder.Services.AddSingleton<IMissionAsignTableOperate, MissionAsignTableLibrary>();
            builder.Services.AddSingleton<IPierMissionTableOperate, PierMissionTableLibrary>();
            builder.Services.AddSingleton<IRobotMissionTableOperate, RobotMissionTableLibrary>();

            #endregion

            #region CommonService

            builder.Services.AddSingleton<ObserverService>();

            #endregion

            #region Scope

            builder.Services.AddSingleton<MachineScope>();

            #endregion

            #region Service

            builder.Services.AddSingleton<IMachineService, MachineService>();

            #endregion

            return builder;
        }
    }
}
