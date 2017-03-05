using System.Collections.Generic;
using System.Net.NetworkInformation;
using FloridSword.SystemService.Tools;
using ServiceStack;

namespace FloridSword.SystemService.Services
{
    internal class NetworkService : Service
    {
        private readonly INetworkTool _networkTool;

        public NetworkService(INetworkTool networkTool)
        {
            _networkTool = networkTool;
        }

        public NetworkInterfaceResponse Get(NetworkInterfaceRequest request)
        {
            var cards =
                _networkTool.GetAllNetworkInterfaces()
                    .ConvertAll(i => i.ConvertTo<NetworkInterfaceDto>());

            return new NetworkInterfaceResponse
            {
                Cards = cards
            };
        }
    }

    [Route("/network/interface")]
    public class NetworkInterfaceRequest : IReturn<NetworkInterfaceResponse>
    {
    }

    public class NetworkInterfaceResponse
    {
        public List<NetworkInterfaceDto> Cards { get; set; }
    }

    public class NetworkInterfaceDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OperationalStatus OperationalStatus { get; set; }
        public bool IsReceiveOnly { get; set; }
        public NetworkInterfaceType NetworkInterfaceType { get; set; }
        public long Speed { get; set; }
        public bool SupportsMulticast { get; set; }
    }
}
