using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace FloridSword.SystemService.Tools
{
    internal interface INetworkTool
    {
        List<NetworkInterface> GetAllNetworkInterfaces();
        bool GetIsNetworkAvailable();
    }
}
