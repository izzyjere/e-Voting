using ICTAZEVoting.BlockChain.IO;
using ICTAZEVoting.BlockChain.Network;
using ICTAZEVoting.Core.Extensions;
using ICTAZEVoting.Shared.Wrapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;

using System.Xml.Linq;

namespace ICTAZEVoting.Api.Hubs
{
    public class BlockChainHub : Hub<INodeClient>
    {
        readonly ILogger<BlockChainHub> logger;
        public BlockChainHub(ILogger<BlockChainHub> logger)
        {
            this.logger = logger;
        }

        public async Task SendMessage(NetworkMessage message)
              => await Clients.All.ReceiveMessage(message);
        public override async Task OnConnectedAsync()
        {
            var nodeAddress = GetRequestIP(Context.GetHttpContext());
            var node = new Node(nodeAddress);
            Clients.Caller.NodeId = node.NodeId;
            await Clients.Others.NodeConnected(node);
            logger.LogInformation($"New node connected: at : {node}");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Others.NodeDisconnected(Clients.Caller.NodeId);
            logger.LogInformation($"Node disconnected: at : {Clients.Caller.NodeId}");
            await base.OnDisconnectedAsync(exception);
        }
        public static string GetRequestIP(HttpContext context, bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = context.GetHeaderValueAs("X-Forwarded-For").SplitCsv().FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && context.Connection?.RemoteIpAddress != null)
                ip = context.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = context.GetHeaderValueAs("REMOTE_ADDR");

            // context.Request?.Host this is the local host.

            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }


    }
}
