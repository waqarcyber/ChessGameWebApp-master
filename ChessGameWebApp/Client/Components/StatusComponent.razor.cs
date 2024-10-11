using ChessGameClient.Services;
using ChessGame;
using ChessGameWebApp.Client.Services;
using Microsoft.AspNetCore.Components;

namespace ChessGameWebApp.Client.Components
{
    public class StatusComponentModel : ComponentBase, IChessObserver
    {
        public IPlayer Player { get; set; }
        public IPlayer Opponent { get; set; }
        [Parameter]
        public string Status { get; set; }
        [Inject]
        public IGameHubService _GameHubService { get; set; }
        private ChessBoard board;
        [Parameter]
        public ChessBoard Board
        {
            get => board;
            set
            {
                board = value;
            }
        }
        public async Task UpdateAsync()
        {
            Status = board.GameStatusDescription;

            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            if (Board.Player != null && Board.Players != null)
            {
                Player = (Player)Board.Player;
                Opponent = (Player)Board.Players.First(p => p != Player);
            }
            ((IChessObservable)Board).Subscribe(this);
        }

        public void Dispose()
        {
            ((IChessObservable)Board).Remove(this);
        }
    }
}
