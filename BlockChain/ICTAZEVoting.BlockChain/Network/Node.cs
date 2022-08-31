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
        public Node(string address)
        {
            Uri = address;
            
        }
        public string Uri { get; }    
        public string NodeId { get; set; }
    }
}
