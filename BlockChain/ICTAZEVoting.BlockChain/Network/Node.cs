using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace ICTAZEVoting.BlockChain.Network
{
    public class Node 
    {
        public Guid NodeId { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
    }
}
