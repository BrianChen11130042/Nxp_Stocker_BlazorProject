using NXP_Stocker_BlazorProject.Services;
using NXP_Stocker_BlazorProject.Components;
using Microsoft.Extensions.Hosting.WindowsServices;
using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using CommonLibraryB_NXP.Tools.LogWritter;
using CommonLibraryB_NXP.Manager.ModbusTcp.Master;
using CommonLibraryB_NXP.Library.PLC.Config;
using NXP_Stocker_BlazorProject.DeviceName.PLC;
using CommonLibraryB_NXP.Library.PLC.Property;
using CommonLibraryB_NXP.Library.PLC;
using NXP_Stocker_BlazorProject.Scope;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.Services.Interface;
using CommonLibraryB_NXP;
using NXP_Stocker_BlazorProject;
using NXP_Stocker_BlazorProject.DeviceName.UPS;

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

builder.AddCommonLibraryB<EPLC, EUPS>(filePath);

builder.AddNxpServiceB();

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