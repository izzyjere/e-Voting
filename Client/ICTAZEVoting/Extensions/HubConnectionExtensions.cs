using ICTAZEVoting.BlockChain.Network;

using Microsoft.AspNetCore.SignalR.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Extensions
{
    public static class HubConnectionExtensions
    {
        public static IDisposable OnMessageReceived(
           this HubConnection connection, Func<NetworkMessage, Task> handler) =>
           connection.On("ReceiveMessage", handler);

        public static IDisposable OnNodeConnected(
            this HubConnection connection, Func<Node, Task> handler) =>
            connection.On("NodeConnected", handler);

        public static IDisposable OnNodeDisconneted(
            this HubConnection connection, Func<string, Task> handler) =>
            connection.On("NodeDisconnected", handler);
        
    }
}
