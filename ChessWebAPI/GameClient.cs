using ChessGameClient.AuthWebAPI;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ChessGame;
using ChessGameClient.Models;
using ChessGameClient.Services.Impl;
using ChessGameClient.Services;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;

namespace ChessGameClient
{
    public class GameClient
    {
        readonly protected string configFile = "settings.json";

        private static GameClient client;
        public static GameClient Client
        {
            get 
            {
                if (client == null)
                {
                    client = new GameClient();
                }

                return client;
            } 
        }
        protected readonly WindsorContainer container = new WindsorContainer();

        public readonly GameHttpClient gameHttpClient;
        public readonly AuthHttpClient authHttpClient;
        public readonly IAuthWebApi authWebApi;
        public readonly ChessBoard chessBoard;
        public readonly IGameHubService gameHubService;
        public readonly IChatHubService chatHubService;
        public readonly SiteUserInfo siteUserInfo;
        public readonly TimeUpdater timeUpdater;
        public readonly IMyLocalStorageService myLocalStorage;
        public readonly IAuthService authService;
        public readonly List<ChatMessage> messages;

        protected GameClient()
        {
            container.Register(Component.For<GameHttpClient>());
            gameHttpClient = container.Resolve<GameHttpClient>();

            container.Register(Component.For<AuthHttpClient>());
            authHttpClient = container.Resolve<AuthHttpClient>();

            var settings = GetSettings();
            if (settings != null)
            {
                authHttpClient.BaseAddress = new Uri(settings.AuthClient);
                gameHttpClient.BaseAddress = new Uri(settings.GameClient);
            }

            container.Register(Component.For<IAuthWebApi>()
                .ImplementedBy<AuthWebApi>());
            authWebApi = container.Resolve<IAuthWebApi>();

            container.Register(Component.For<List<ChatMessage>>());
            messages = container.Resolve<List<ChatMessage>>();

            container.Register(Component.For<TimeUpdater>());
            timeUpdater = container.Resolve<TimeUpdater>();

            container.Register(Component.For<SiteUserInfo>());
            siteUserInfo = container.Resolve<SiteUserInfo>();

            container.Register(Component.For<ChessBoard>());
            chessBoard = container.Resolve<ChessBoard>();

            container.Register(Component.For<IGameHubService>()
                .ImplementedBy(GetGameHubServiceType()));
            gameHubService = container.Resolve<IGameHubService>();

            container.Register(Component.For<IChatHubService>()
                .ImplementedBy<ChatHubServiceImpl>());
            chatHubService = container.Resolve<IChatHubService>();

            container.Register(Component.For<IMyLocalStorageService>()
                .ImplementedBy<MyLocalStorageServiceImpl>());
            myLocalStorage = container.Resolve<IMyLocalStorageService>();

            container.Register(Component.For<IAuthService>()
                .ImplementedBy(GetAuthServiceType()));
            authService = container.Resolve<IAuthService>();

        }

        protected ClientSettings? GetSettings()
        {
            var content = File.ReadAllText(configFile);

            return JsonSerializer.Deserialize<ClientSettings>(content);
        }
        public virtual Type GetAuthServiceType() => typeof(AuthServiceImpl);

        public virtual Type GetGameHubServiceType() => typeof(GameHubServiceImpl);
    } 
}
