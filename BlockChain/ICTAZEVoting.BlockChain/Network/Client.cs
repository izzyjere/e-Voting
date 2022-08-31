﻿using ICTAZEVoting.BlockChain.IO;
using ICTAZEVoting.Shared.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using ICTAZEVoting.Shared.Wrapper;

namespace ICTAZEVoting.BlockChain.Network
{
    public class Client
    {
        public static void Connect(string nodeAddress)
        {
            if (!NodeService.Nodes.ContainsKey(nodeAddress))
            {
                var server = new WebSocketSharp.WebSocket(nodeAddress);
                server.OnMessage += (sender, e) =>
                {
                    var message = JsonConvert.DeserializeObject<NetworkMessage>(e.Data);
                    if (message.Type == MessageType.Greeting) 
                    {
                       
                        Console.WriteLine("Response Received.");
                        
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

                    }
                };
                server.Connect();
                server.Send(JsonConvert.SerializeObject(new NetworkMessage { Type = MessageType.Greeting, Payload = JsonConvert.SerializeObject(NodeService.NodeInstance) }));
                server.Send(JsonConvert.SerializeObject(new NetworkMessage { Type = MessageType.BlockChain, Payload = JsonConvert.SerializeObject(NodeService.Storage.GetBlockChain()) }));
                NodeService.Add(nodeAddress, server);
            }
        }
        public static void Send(string url, string data)
        {
            foreach (var item in NodeService.Nodes)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public static void Broadcast(string data)
        {
            foreach (var item in NodeService.Nodes)
            {
                item.Value.Send(data);
            }
        }

        public static IList<string> GetServerAddresses()
        {
            IList<string> addresses = new List<string>();
            foreach (var item in NodeService.Nodes)
            {
                addresses.Add(item.Key);
            }
            return addresses;
        }
        public static void Close()
        {
            foreach (var item in NodeService.Nodes)
            {
                item.Value.Close();
            }
        }

    }
}
