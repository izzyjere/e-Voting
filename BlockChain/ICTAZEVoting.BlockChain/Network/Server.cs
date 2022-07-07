using ICTAZEvoting.Shared.Models;

using ICTAZEVoting.BlockChain.IO;
using ICTAZEVoting.BlockChain.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace ICTAZEVoting.BlockChain.Network
{
    public class Server : WebSocketBehavior
    {
        
        bool chainSynced = false;
        WebSocketServer server = null;

        public Server()
        {

        }    
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Node.")
            {
                Console.WriteLine(e.Data);
                Send("Hi too Node.");
            }
            else
            {
                var newBlockChain = JsonConvert.DeserializeObject<Models.BlockChain>(e.Data);
                var myChain = NodeService.Storage.GetBlockChain();
                var newVotes = new List<Vote>();
                //Check block chain validity
                if (newBlockChain.IsValid()&&newBlockChain.Chain.Count>myChain.Chain.Count)
                {
                    var newVote = newBlockChain.PendingVote;
                    if(newVote != null)
                    {
                        myChain.PendingVote = newVote;
                        NodeService.Storage.UpdateBlockChain(myChain);
                    }
                   
                }
                if(!chainSynced)
                {
                    Send(JsonConvert.SerializeObject(NodeService.Storage.GetBlockChain()));
                    chainSynced = true;
                }
            }
        }
        public string GetNodeId()
        {
            return NodeService.NodeInstance.NodeId.ToString();
        }
        public string GetIpAddress()
        {
            return NodeService.NodeInstance.IPAddress + $":{NodeService.NodeInstance.Port}";
        }
        public void Start()
        {
            server = new WebSocketServer($"{NodeService.NodeInstance.IPAddress}:{NodeService.NodeInstance.Port}");
            server.AddWebSocketService<Server>("/ICTAZEVoting.Blockchain");
            server.Start();
            Console.WriteLine($"Node has been created and started on: {GetIpAddress()}", Environment.NewLine);
        }

    }
}
