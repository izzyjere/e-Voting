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
        readonly Node node;
        readonly StorageContext storageContext;
        bool chainSynced = false;
        WebSocketServer server = null;
        public Server()
        {

        }
        public Server(Node node, StorageContext storageContext=null)
        {
            this.node = node;
            this.storageContext = storageContext;

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
                var myChain = storageContext.GetBlockChain();
                var newVotes = new List<Vote>();
                //Check block chain validity
                if (newBlockChain.IsValid()&&newBlockChain.Chain.Count>myChain.Chain.Count)
                {
                    var newVote = newBlockChain.PendingVote;
                    if(newVote != null)
                    {
                        myChain.PendingVote = newVote;
                        storageContext.UpdateBlockChain(myChain);
                    }
                   
                }
                if(!chainSynced)
                {
                    Send(JsonConvert.SerializeObject(storageContext.GetBlockChain()));
                    chainSynced = true;
                }
            }
        }
        public string GetNodeId()
        {
            return node.NodeId.ToString();
        }
        public string GetIpAddress()
        {
            return node.IPAddress + $":{node.Port}";
        }
        public void Start()
        {
            server = new WebSocketServer($"{node.IPAddress}:{node.Port}");
            server.AddWebSocketService<Server>("/ICTAZEVoting.Blockchain");
            server.Start();
            Console.WriteLine($"Node has been created and started on: {GetIpAddress()}", Environment.NewLine);
        }

    }
}
