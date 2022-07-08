using ICTAZEvoting.Shared.Models;
using ICTAZEvoting.Shared.Wrapper;

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
        protected override void OnMessage(MessageEventArgs e)
        {
            var message = JsonConvert.DeserializeObject<NetworkMessage>(e.Data);
            if (message.Type == MessageType.Greeting)
            {
                var node = JsonConvert.DeserializeObject<Node>(message.Payload);
                NodeService.Add(node.IPAddress, new WebSocket(node.IPAddress));
                Console.WriteLine("New Node Registered");
                Send("Hi too Node.");
            }
            else 
            {
                var newBlockChain = JsonConvert.DeserializeObject<Models.BlockChain>(message.Payload);
                var myChain = NodeService.Storage.GetBlockChain();              
                //Check block chain validity
                if (newBlockChain.IsValid()&&newBlockChain.Chain.Count>myChain.Chain.Count)
                {
                    var difference = newBlockChain.Chain.Count - myChain.Chain.Count;                     
                }
                if(!chainSynced)
                {
                    Send(JsonConvert.SerializeObject(NodeService.Storage.GetBlockChain()));
                    chainSynced = true;
                }
            }
        }
        public static string GetNodeId()
        {
            return NodeService.NodeInstance.NodeId.ToString();
        }
        public static string GetIpAddress()
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
