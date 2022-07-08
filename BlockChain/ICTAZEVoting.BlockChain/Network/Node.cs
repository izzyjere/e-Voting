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
        public Node(string address, int port)
        {

        }
        public string IPAddress { get; }
        public int Port { get;  }        
    }
}
