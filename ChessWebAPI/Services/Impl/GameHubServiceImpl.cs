using ChessGameClient;
using ChessGameClient.AuthWebAPI;
using ChessGameClient.Models;
using ChessGame;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Http.Headers;

namespace ChessGameClient.Services.Impl
{
    public class GameHubServiceImpl : IGameHubService, IAsyncDisposable
    {
        protected readonly ChessBoard _board;
        protected readonly HubConnection hubConnection;
        protected readonly SiteUserInfo _siteUserInfo;

        protected readonly GameHttpClient _httpClient;
        public GameHubServiceImpl(
                              ChessBoard board,
                              GameHttpClient httpClient,
                              SiteUserInfo siteUserInfo)
        {
            _board = board ?? throw new ArgumentNullException(nameof(board));
            _siteUserInfo = siteUserInfo ?? throw new ArgumentNullException(nameof(siteUserInfo));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _board.SetCheckMethod(TryMove);

            hubConnection = new HubConnectionBuilder()
                .WithUrl(@$"{_httpClient.BaseAddress.OriginalString.Trim('/')}/gamehub", options =>
                 {
                     options.AccessTokenProvider = () => Task.FromResult(_httpClient.DefaultRequestHeaders.Authorization?.Parameter);
                 })
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<ChessBoardDto, FigureColor, ChessTimerDto>("ReceiveBoard", (board, playerColor, timer) =>
            {
                _board.SetCurrentPlayer(playerColor);
                _board.Update(board);
                _board.SetTimer(timer);

                OnReceiveBoardAction();
            });

            hubConnection.On<ChessCellDto, ChessCellDto, ChessTimerDto>("ReceiveTryMove", (from, to, timer) =>
            {
                var fromCell = _board.GetCell(from.Row, from.Column);
                var toCell = _board.GetCell(to.Row, to.Column);

                _board.TryMove(fromCell, toCell);

                if (timer != null)
                    _board.SetTimer(timer);
            });

            hubConnection.On<bool>("StartGame", (start) =>
            {
                if (start)
                    GameStartAction();
                    //navigationManager.NavigateTo("/Game/start");
            });

            hubConnection.On<bool>("ReceiveMoveBack", (ok) =>
            {
                if (ok)
                    _board.TryMoveBack();
            });

            hubConnection.On<int, bool>("ChangeStatus", (id, status) =>
            {
                lock (_siteUserInfo)
                {
                    if (_siteUserInfo.Id == id)
                        _siteUserInfo.Status = status;
                    else
                    {
                        _siteUserInfo.RivalStatus = status;
                    }
                }
            });

            hubConnection.On<int, string>("GetInvite", (id, rivalName) =>
            {
                _siteUserInfo.RivalId = id;
                _siteUserInfo.RivalName = rivalName;
                GetInviteAction();
            });

            hubConnection.On("CloseInvite", () =>
            {
                CloseInviteAction();
            });

            hubConnection.On("SendGameStatus", (GameStatus status) =>
            {
                _board.GameStatus = status;

                if (_board.GameStatus == GameStatus.GiveUp || _board.GameStatus == GameStatus.OpponentGiveUp)
                    _board.СhessСlock.Stop();
            });

            InitConnection();
        }

        public virtual void OnReceiveBoardAction() { }
        public virtual void GameStartAction() { }
        public virtual void GetInviteAction() { }

        public virtual void CloseInviteAction() { }
        public async Task<bool> InitConnection()
        {
            try
            {
                await StartGame();
            }
            catch { }

            return IsConnected;
        }
        public async Task MoveBack()
        {
            if (!IsConnected)
                await hubConnection.StartAsync();
            await hubConnection.SendAsync("MoveBack");
        }

        public async Task GetBoard()
        {
            if (!IsConnected)
                await hubConnection.StartAsync();
            await hubConnection.SendAsync("SendBoard");
        }

        public async Task SendTryMove(ChessCellDto from, ChessCellDto to)
        {
            if (!IsConnected)
                await hubConnection.StartAsync();
            await hubConnection.SendAsync("SendTryMove", from, to);
        }

        public async Task StartGame()
        {
            if (!IsConnected)
                await hubConnection.StartAsync();
            await hubConnection.SendAsync("StartGame", false);
        }

        public async Task GameOver()
        {
            if (!IsConnected)
                await hubConnection.StartAsync();
            await hubConnection.SendAsync("GameOver");
        }

        public async Task AddOrRemovePlayer(int rivalId = 0)
        {
            if (!IsConnected)
                await hubConnection.StartAsync();
            await hubConnection.SendAsync("AddOrRemovePlayer", rivalId);
        }

        public async Task SendInvite(int rivalId, string rivalName)
        {
            if (!IsConnected)
                await hubConnection.StartAsync();
            await hubConnection.SendAsync("SendInvite", rivalId, rivalName);
        }

        public async Task CloseInvite(int rivalId)
        {
            if (!IsConnected)
                await hubConnection.StartAsync();
            await hubConnection.SendAsync("CloseInvite", rivalId);
        }

        public bool IsConnected
        { get => hubConnection.State == HubConnectionState.Connected; }


        public Task Help()
        {
            _board.GetHelp();

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection != null)
            {
                await hubConnection.StopAsync();
                //await hubConnection.DisposeAsync();
            }
        }

        public async Task<bool> TryMove(Cell from, Cell to)
        {
            await SendTryMove(from.ToDto(), to.ToDto());
            return false;
        }
    }
}
