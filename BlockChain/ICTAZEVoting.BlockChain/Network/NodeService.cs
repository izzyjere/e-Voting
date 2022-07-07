using ICTAZEVoting.BlockChain.IO;

using WebSocketSharp;

namespace ICTAZEVoting.BlockChain.Network
{
    public static class NodeService
    {
        public static Dictionary<string, WebSocket> Nodes { get; set; }
        public static void Add(string nodeAddress, WebSocket server)
        {
            if(!Nodes.ContainsKey(nodeAddress))
                Nodes.Add(nodeAddress, server);
        }
        public static StorageContext  Storage { get; set; }
        public static Node NodeInstance { get; set; }
    }
}
