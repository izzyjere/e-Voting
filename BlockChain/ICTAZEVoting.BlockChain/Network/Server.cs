using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Wrapper;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ICTAZEVoting.BlockChain.Network;

public class Server : WebSocketBehavior
{
    
    bool chainSynced = false;
    static WebSocketServer server = new();
    private readonly int portNumber;
    Logger logger = new();
    string address;
    public Server(int portNumber, string address)
    {
        this.portNumber = portNumber;
        this.address = address;
        logger.Level = LogLevel.Info;
    }
    public Server()
    {
        logger.Level = LogLevel.Info;            
    }
    protected override void OnMessage(MessageEventArgs e)
    {
        if(e.IsPing)
        {
            logger.Info("Some one pinged me");
        }
        var message = JsonConvert.DeserializeObject<NetworkMessage>(e.Data);
        if (message.Type == MessageType.Greeting)
        {
            var node = JsonConvert.DeserializeObject<Node>(message.Payload);
            NodeService.Add(node.Uri, new WebSocket(node.Uri));
            logger.Info($"Node has been created and started on: {GetIpAddress()}\n");
            Send(JsonConvert.SerializeObject(new NetworkMessage { Type=MessageType.Greeting,Payload=JsonConvert.SerializeObject(NodeService.NodeInstance)}));
        }
        else 
        {
            var newBlockChain = JsonConvert.DeserializeObject<Models.BlockChain>(e.Data);
            var myChain = NodeService.Storage.GetBlockChain();

            //Check block chain validity
            if (newBlockChain.IsValid() && newBlockChain.Chain.Count > myChain.Chain.Count)
            {
                //:TODO
                var newBallots = new List<Ballot>();
                newBallots.AddRange(newBlockChain.PendingBallots);
                newBallots.AddRange(myChain.PendingBallots);
                newBlockChain.PendingBallots = newBallots;
                NodeService.Storage.UpdateBlockChain(newBlockChain);

            }
            if (!chainSynced)
            {
                Send(JsonConvert.SerializeObject(NodeService.Storage.GetBlockChain()));
                chainSynced = true;
            }
        }
    }        
    public  string GetIpAddress()
    {
        return server.Address.ToString();
    }
    public void Start()
    {
        server = new WebSocketServer($"{address}:{portNumber}");
        server.AddWebSocketService<Server>("/Blockchain");
        server.Start();         
        logger.Info($"Listening on {GetIpAddress()}");
      
    }

}
