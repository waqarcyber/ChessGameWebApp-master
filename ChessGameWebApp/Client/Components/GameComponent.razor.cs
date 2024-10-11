using ChessGameClient.Services;
using ChessGame;
using ChessGameWebApp.Client.Services;
using Microsoft.AspNetCore.Components;

namespace ChessGameWebApp.Client.Components
{
    public class GameComponentModel : ComponentBase, IDisposable, IChessObserver
    {
        public bool Inversion { get => Board.Player?.Color == FigureColor.Black; }

        [Inject]
        public IGameHubService _GameHubService { get; set; }
        public CellComponentModel Target { get; set; }
        [Inject]
        public ChessBoard Board { get; set; }
        public List<CellComponent> Children { get; set; }
        public GameComponentModel()
        {
            Children = new List<CellComponent>();
        }

        protected override async Task OnInitializedAsync()
        {
            await _GameHubService.GetBoard();
        }

        protected override void OnInitialized()
        {
            ((IChessObservable)Board).Subscribe(this);
        }

        public void Dispose()
        {
            ((IChessObservable)Board).Remove(this);
        }

        public Task UpdateAsync()
        {
            StateHasChanged();

            return Task.CompletedTask;
        }
    }
}
