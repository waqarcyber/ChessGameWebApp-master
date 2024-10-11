using ChessGameClient;
using ChessGameClient.AuthWebAPI;
using ChessGameClient.Models;
using Blazored.Modal;
using Blazored.Modal.Services;
using ChessGame;
using ChessGameWebApp.Client.Components;
using Microsoft.AspNetCore.Components;

namespace ChessGameClient.Services.Impl
{
    public class GameHubServiceImplV2 : GameHubServiceImpl
    {
        private IModalReference modalReferense;
        private readonly IModalService _modal;
        private readonly NavigationManager _navigationManager;
        public GameHubServiceImplV2(
                                    ChessBoard board,
                                    NavigationManager navigationManager,
                                    GameHttpClient httpClient,
                                    SiteUserInfo siteUserInfo,
                                    IModalService modal)
                            : base(
                                   board,
                                   httpClient,
                                   siteUserInfo)
        {
            _modal = modal ?? throw new ArgumentNullException(nameof(modal));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
        }

        public override void GetInviteAction()
        {
            modalReferense = _modal.Show<InviteComponent>("Invite");
        }

        public override void CloseInviteAction()
        { 
            modalReferense?.Close(); 
        }

        public override void GameStartAction()
        {
            _navigationManager.NavigateTo("/Game/start");
        }
    }
}
