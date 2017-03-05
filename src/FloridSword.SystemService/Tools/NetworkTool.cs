using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace FloridSword.SystemService.Tools
{
    internal class NetworkTool : INetworkTool
    {
        public List<NetworkInterface> GetAllNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces().ToList();
        }

        public bool GetIsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
