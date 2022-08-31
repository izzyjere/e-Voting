
using ICTAZEVoting.Shared.Wrapper;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.BlockChain.Network
{
    public interface INodeClient
    {
        Task NodeConnected(Node node);      
        Task NodeDisconnected(string nodeId);    
        Task ReceiveMessage(NetworkMessage message);
    }
}
