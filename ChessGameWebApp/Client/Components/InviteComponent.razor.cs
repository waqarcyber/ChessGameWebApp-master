using ChessGameClient.Models;
using ChessGameClient.Services;
using ChessGame;
using ChessGameWebApp.Client.Services;
using Microsoft.AspNetCore.Components;

namespace ChessGameWebApp.Client.Components
{
    public class InviteComponentModel : ComponentBase, IChessObserver, IDisposable
    {
        [Inject]
        public IGameHubService GameHubService { get; set; }
        [Inject]
        public SiteUserInfo User { get; set; }
        public Task UpdateAsync()
        {
            StateHasChanged();

            return Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            ((IChessObservable)User).Subscribe(this);
        }

        public void Dispose()
        {
            ((IChessObservable)User).Remove(this);
        }
    }
}
