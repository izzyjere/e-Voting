using ICTAZEVoting.BlockChain.IO;

using WebSocketSharp;

namespace ICTAZEVoting.BlockChain.Network
{
    public static class NodeService
    {
        public static Dictionary<string, WebSocket> Nodes { get; }
        public static void Add(string nodeAddress, WebSocket server)
        {
            if(!Nodes.ContainsKey(nodeAddress)|| !nodeAddress.Equals(NodeInstance.IPAddress))
                Nodes.Add(nodeAddress, server);
        }
        public static StorageContext  Storage { get; private set; }
        public static Node NodeInstance { get; private set; }
        public static void InitializeNode(string storagePath, string IpAddress, int portNumber)
        {
            NodeInstance = new Node { IPAddress = IpAddress, Port = portNumber, NodeId = Guid.NewGuid() };
            //:TODO  Register node with the network

        }
    }
}
