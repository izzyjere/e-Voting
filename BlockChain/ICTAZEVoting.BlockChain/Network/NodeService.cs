using ICTAZEVoting.BlockChain.IO;

using Newtonsoft.Json;

using WebSocketSharp;

namespace ICTAZEVoting.BlockChain.Network
{
    public static class NodeService
    {
        public static Dictionary<string, WebSocket> Nodes { get; } = new();
        static Server server;
        public static void Add(string nodeAddress, WebSocket server)
        {
            if(!Nodes.ContainsKey(nodeAddress)|| !nodeAddress.Equals(NodeInstance.IPAddress))
                Nodes.Add(nodeAddress, server);
        }
        public static StorageContext  Storage { get; private set; }
        public static Node NodeInstance { get; private set; }
        public static void InitializeNode(string storagePath, string address, int portNumber)
        {
            server = new Server(portNumber,address);
            NodeInstance = new Node (server.GetIpAddress(),portNumber);           
            Storage = new(storagePath);
            server.Start();
            //:TODO  Register node with the network
            RegisterNode();

        }
        static void RegisterNode()
        {
            foreach (var item in Nodes)
            {
                item.Value.Send(JsonConvert.SerializeObject(NodeInstance));
            }
        }
        
    }
}
