using ChessGameClient.AuthWebAPI;
using ChessGameClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChessGameClient.Services.Impl
{
    public class ChatHubServiceImpl : IChatHubService
    {

        private readonly HubConnection _hubConnection;
        private readonly GameHttpClient _httpClient;

        public delegate Task Updater();
        private Updater _updater;

        public ChatHubServiceImpl(
                              GameHttpClient httpClient,
                              List<ChatMessage> messages)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(@$"{_httpClient.BaseAddress.OriginalString.Trim('/')}/chathub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_httpClient.DefaultRequestHeaders.Authorization?.Parameter);
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<ChatMessage>("ReceiveMessage", (param) =>
            {
                var encodedMsg = $"{param.Time:hh\\:mm\\:ss} {param.Username}: {param.Message}";
                messages.Insert(0, param);

                if (_updater != null)
                    _updater();
            });
        }

        public void SetUpdater(Updater updater) => _updater = updater;

        public async Task Start()
        {
            if (!IsConnected)
            {
                await _hubConnection.StartAsync();
            }
        }

        public async Task Send(string message)
        {
            if (!IsConnected)
                await _hubConnection.StartAsync();

            await _hubConnection.SendAsync("SendMessage", message);
        }

        public bool IsConnected =>
            _hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                //await _hubConnection.DisposeAsync();
            }
        }
    }
}
