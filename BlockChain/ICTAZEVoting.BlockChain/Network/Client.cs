using ICTAZEVoting.BlockChain.IO;
using ICTAZEvoting.Shared.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.BlockChain.Network
{
    public class Client
    {
        IDictionary<string, WebSocketSharp.WebSocket> servers = new Dictionary<string, WebSocketSharp.WebSocket>();
        private StorageContext storageContext;
        private bool chainSynced;

        public void Connect(string nodeAddress)
        {
            if (!servers.ContainsKey(nodeAddress))
            {
                var server = new WebSocketSharp.WebSocket(nodeAddress);
                server.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Hi too Node.")
                    {
                        Console.WriteLine(e.Data);
                    }
                    else
                    {
                        var newBlockChain = JsonConvert.DeserializeObject<Models.BlockChain>(e.Data);
                        var myChain = storageContext.GetBlockChain();
                        var newVotes = new List<Vote>();
                        //Check block chain validity
                        if (newBlockChain.IsValid() && newBlockChain.Chain.Count > myChain.Chain.Count)
                        {
                            var newVote = newBlockChain.PendingVote;
                            if (newVote != null)
                            {
                                myChain.PendingVote = newVote;
                                storageContext.UpdateBlockChain(myChain);
                            }

                        }

                    }
                };
                server.Connect();
                server.Send("Hi Server");
                server.Send(JsonConvert.SerializeObject(storageContext.GetBlockChain()));
                servers.Add(nodeAddress, server);
            }
        }
        public void Send(string url, string data)
        {
            foreach (var item in servers)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in servers)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServerAddresses()
        {
            IList<string> addresses = new List<string>();
            foreach (var item in servers)
            {
                addresses.Add(item.Key);
            }
            return addresses;
        }
        public void Close()
        {
            foreach (var item in servers)
            {
                item.Value.Close();
            }
        }

    }
}
