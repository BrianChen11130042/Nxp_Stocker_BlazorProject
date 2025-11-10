using NXP_Stocker_BlazorProject.Services;
using NXP_Stocker_BlazorProject.Components;
using Microsoft.Extensions.Hosting.WindowsServices;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using CommonLibraryB.Tools.LogWritter;
using CommonLibraryB.Manager.ModbusTcp.Master;
using CommonLibraryB_NXP.Library.PLC.Config;
using NXP_Stocker_BlazorProject.DeviceName.PLC;
using CommonLibraryB_NXP.Library.PLC.Property;
using CommonLibraryB_NXP.Library.PLC;
using NXP_Stocker_BlazorProject.Scope;
using NXP_Stocker_BlazorProject.CommonService.Observer;

//var builder = WebApplication.CreateBuilder(args);

//指定在Windows Service」環境下正確運作
var webApOpts = new WebApplicationOptions
{
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ?
        AppContext.BaseDirectory : default,
    Args = args
};
var builder = WebApplication.CreateBuilder(webApOpts);
builder.Host.UseWindowsService();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMvc();

string filePath = System.AppDomain.CurrentDomain.BaseDirectory;

#region DB Table

builder.Services.AddSingleton<ILogTableOperate, LogTableLibrary>();
builder.Services.AddSingleton<IMissionAsignTableOperate, MissionAsignTableLibrary>();
builder.Services.AddSingleton<IPierMissionTableOperate, PierMissionTableLibrary>();
builder.Services.AddSingleton<IRobotMissionTableOperate, RobotMissionTableLibrary>();
builder.Services.AddSingleton<IWarehouseTableOperate, WarehouseTableLibrary>();

#endregion

#region CommonService

builder.Services.AddSingleton<ObserverService>();

#endregion

#region Tools

builder.Services.AddSingleton<LogWritter>(provider => new LogWritter(filePath));

#endregion

#region Manager

builder.Services.AddSingleton<ModbusTcpMasterManager>(provider => new ModbusTcpMasterManager(filePath));

#endregion

#region Library

builder.Services.AddSingleton<PlcConfigManager<EPLC>>(provider => new PlcConfigManager<EPLC>(filePath));
builder.Services.AddSingleton<PlcPropertyManager<EPLC>>(provider => new PlcPropertyManager<EPLC>(filePath));
builder.Services.AddSingleton<PlcLibrary<EPLC>>();

#endregion

#region Scope

builder.Services.AddSingleton<Scope>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();