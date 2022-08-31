﻿using ICTAZEVoting.BlockChain.IO;
using ICTAZEVoting.BlockChain.Network;

using Microsoft.AspNetCore.SignalR.Client;

using Newtonsoft.Json;
namespace ICTAZEVoting.Services.Domain
{
    public class NodeConnectionInstance : IAsyncDisposable
    {
         HubConnection? hubConnection;
        readonly HashSet<IDisposable> hubRegistrations = new();
        Dictionary<string, Node> ConnectedNodes { get; set; } = new();
        readonly StorageContext storageContext;
        bool chainSynced;
        Task RegisterNode(Node node)
        {
            ConnectedNodes.Add(node.NodeId, node);
            return Task.CompletedTask;
        }
        Task RemoveNode(string id)
        {
            if (ConnectedNodes.ContainsKey(id))
            {
                ConnectedNodes.Remove(id);
            }
            return Task.CompletedTask;
        }
        public NodeConnectionInstance()
        {
            var appPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            storageContext = new StorageContext(Path.Combine(appPath,"Evoting","data"));
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7119/blockchain").WithAutomaticReconnect().Build();
            hubRegistrations.Add(hubConnection.OnMessageReceived(OnMessageRecievedAsync));
            hubRegistrations.Add(hubConnection.OnNodeConnected(node =>
                RegisterNode(node)));
            hubRegistrations.Add(hubConnection.OnNodeDisconneted(nodeId =>
                RemoveNode(nodeId)));
            storageContext.InitializeBlockChain();
            BuildConnectionAsync();
        }
         async void BuildConnectionAsync() 
        {
            if (hubConnection != null)
            {
                await hubConnection.StartAsync();
            }     
        }
        async Task OnMessageRecievedAsync(NetworkMessage message)
        {
            var newBlockChain = JsonConvert.DeserializeObject<BlockChain.Models.BlockChain>(message.Payload);
            var myChain = storageContext.GetBlockChain();
            //Check block chain validity
            if (newBlockChain.IsValid() && newBlockChain.Chain.Count > myChain.Chain.Count)
            {
                //:TODO
                var newBallots = new List<Ballot>();
                newBallots.AddRange(newBlockChain.PendingBallots);
                newBallots.AddRange(myChain.PendingBallots);
                newBlockChain.PendingBallots = newBallots;
                storageContext.UpdateBlockChain(newBlockChain);
            }
            if (!chainSynced)
            {
                await SendMessageAsync();
            }

        }
        public async Task AddBallotAsync(Ballot ballot)
        {
            storageContext.AddBallot(ballot);
            await SendMessageAsync();
        }
        async Task SendMessageAsync()
        {
            if (hubConnection is not null)
                await hubConnection.InvokeAsync("SendMessage", storageContext.GetBlockChain());
        }
        public async ValueTask DisposeAsync()
        {
            if (hubRegistrations is { Count: > 0 })
            {
                foreach (var disposable in hubRegistrations)
                {
                    disposable.Dispose();
                }
            }

            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }
    }
}