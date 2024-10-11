using ChessGameClient.Models;
using ChessGame;
using ChessGameWebApp.Client.Services;
using Microsoft.AspNetCore.Components;

namespace ChessGameWebApp.Client.Components
{
    public class JWTExpireComponentModel : ComponentBase, IChessObserver, IDisposable
    {
        [Inject]
        public SiteUserInfo User { get; set; }

        [Inject]
        TimeUpdater Updater { get; set; }

        protected override void OnInitialized()
        {
            ((IChessObservable)Updater).Subscribe(this);
        }
        

        public Task UpdateAsync()
        {
            StateHasChanged();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            ((IChessObservable)Updater).Remove(this);
        }
    }
}
