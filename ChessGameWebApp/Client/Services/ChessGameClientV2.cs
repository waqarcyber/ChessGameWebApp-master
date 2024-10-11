using ChessGameClient;
using ChessGameClient.Services.Impl;
using Blazored.Modal.Services;
using Castle.MicroKernel.Registration;
using ChessGameWebApp.Client.Services.Impl;
using Microsoft.AspNetCore.Components;

namespace ChessGameWebApp.Client.Services
{
    public class ChessGameClientV2 : GameClient
    {
        private readonly NavigationManager _navigationManager;
        private readonly IModalService _modalService;
        public ChessGameClientV2(NavigationManager navigationManager, IModalService modalService) : base()
        {
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
            _modalService = modalService ?? throw new ArgumentNullException(nameof(modalService));

            container.Register(Component.For<NavigationManager>()
                .UsingFactoryMethod(GetNavigationManager));
            container.Resolve<NavigationManager>();

            container.Register(Component.For<IModalService>()
                .UsingFactoryMethod(GetModalService));
            container.Resolve<IModalService>();
        }
        public override Type GetAuthServiceType() => typeof(AuthServiceImplV2);

        public override Type GetGameHubServiceType() => typeof(GameHubServiceImplV2);

        private NavigationManager GetNavigationManager() => _navigationManager;
        private IModalService GetModalService() => _modalService;
    }
}
