global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
using Docket.Client;
using Docket.Client.Securities;
using Docket.Client.Services;
using Docket.Client.Services.Contracts;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Docket.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44331/") });
builder.Services.AddMudServices();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IDocketService, DocketService>();
builder.Services.AddScoped<DTODocketUpdate>();
builder.Services.AddScoped<IDocketRateService, DocketRateService>();
builder.Services.AddScoped<IDocketFavoriteService, DocketFavoriteService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
