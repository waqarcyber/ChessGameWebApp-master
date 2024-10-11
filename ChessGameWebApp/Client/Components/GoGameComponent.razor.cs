using ChessGameClient;
using ChessGameClient.AuthWebAPI;
using ChessGameClient.Models;
using ChessGameClient.Services;
using ChessGame;
using ChessGameWebApp.Client.Services;
using Microsoft.AspNetCore.Components;

namespace ChessGameWebApp.Client.Components
{
    public class GoGameComponentModel : ComponentBase, IDisposable, IChessObserver
    {
        [Inject]
        protected SiteUserInfo User { get; set; }
        [Inject]
        protected IGameHubService GameHubService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IAuthWebApi AuthWebApi { get; set; }

        [Parameter]
        public bool RandomRival { get; set; } = true;

        [Parameter]
        public bool IsRival { get; set; } = false;
        public async void StartGame()
        {
            if (IsRival)
                return;

            if (await AuthWebApi.SessionExists())
                NavigationManager.NavigateTo("/game/start");
            else
                await GameHubService.AddOrRemovePlayer(!RandomRival ? User.RivalId : 0);
        }

        protected bool PlayerStatus = false;
        protected override void OnInitialized()
        {
            ((IChessObservable)User).Subscribe(this);
        }

        public void Dispose()
        {
            ((IChessObservable)User).Remove(this);
        }

        public Task UpdateAsync()
        {
            if (IsRival)
                PlayerStatus = User.RivalStatus;
            else
                PlayerStatus = User.Status;

            StateHasChanged();

            return Task.CompletedTask;
        }
    }
}

