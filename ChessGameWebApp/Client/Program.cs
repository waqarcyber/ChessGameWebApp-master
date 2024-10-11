using ChessGame;
using ChessGameWebApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Modal;
using ChessGameWebApp.Client.Services.Impl;
using ChessGameClient.AuthWebAPI;
using ChessGameClient.Services;
using ChessGameClient.Models;
using ChessGameClient.Services.Impl;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredModal();

builder.Services.AddScoped(gameClient => new GameHttpClient() { BaseAddress = new Uri(builder.Configuration["GameClient"])});
builder.Services.AddScoped(authClient => new AuthHttpClient() { BaseAddress = new Uri(builder.Configuration["AuthClient"]) });
builder.Services.AddScoped<IAuthWebApi, AuthWebApi>();
builder.Services.AddScoped<ChessBoard>();
builder.Services.AddScoped<IGameHubService, GameHubServiceImplV2>();
builder.Services.AddScoped<IChatHubService, ChatHubServiceImpl>();
builder.Services.AddScoped<SiteUserInfo>();
builder.Services.AddScoped<TimeUpdater>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IMyLocalStorageService, MyLocalStorageServiceV2>();
builder.Services.AddScoped<IAuthService, AuthServiceImplV2>();
builder.Services.AddScoped<List<ChatMessage>>();

await builder.Build().RunAsync();
