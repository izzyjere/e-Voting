using ICTAZEVoting.BlockChain.IO;
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
        public int NodeCount { get => ConnectedNodes.Count + 1; }
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
        public int CountBallots(Guid? electionId)
        {
            if (electionId == null)
            {
                return 0;
            }
            var list = GetBallots((Guid)electionId);

            return list.Any() ? list.Count : 0;
        }
        public double CountVotes(Guid? electionId, Guid? candidateId)
        {
            if (electionId == null || candidateId == null)
            {
                return 0;
            }
            var allBallots = GetBallots((Guid)electionId);
            if (allBallots.Any())
            {
                var myVotes = allBallots.SelectMany(b => b.Votes.Where(v => v.CandidateId == candidateId));
                if (!myVotes.Any())
                {
                    return 0;
                }
                return myVotes.Count();
            }
            return 0;
        }
        public double CountVotesPerPosition(Guid? electionId, Guid? positionId)
        {
            if (electionId == null || positionId == null)
            {
                return 0;
            }
            var allBallots = GetBallots((Guid)electionId);
            if (allBallots.Any())
            {
                var allVotes = allBallots.SelectMany(b => b.Votes.Where(v => v.PositionId==positionId));
                if (!allVotes.Any())
                {
                    return 0;
                }
                return allVotes.Count();
            }
            return 0;
        }
        public NodeConnectionInstance()
        {
            var appPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            storageContext = new StorageContext(Path.Combine(appPath, "Evoting", "data"));
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
        public List<Ballot> GetBallots(Guid electionId)
        {
            var chain = storageContext.GetBlockChain();
            var ballots = new List<Ballot>();
            if (chain == null)
            {
                return new();
            }
            foreach (var item in chain.Chain.Where(b => b.Data.ElectionId == electionId))
            {
                ballots.Add(item.Data);
            }
            return ballots;
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
