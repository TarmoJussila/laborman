using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public enum IpVersion
{
    IPv4, IPv6
}

public static class IPUtility
{
    public static string GetIP(IpVersion ipVersion)
    {
        if (ipVersion == IpVersion.IPv6 && !Socket.OSSupportsIPv6)
        {
            ipVersion = IpVersion.IPv4; // Fallback to IPv4 if OS does not support IPv6.
        }

        if (ipVersion == IpVersion.IPv4 && !Socket.OSSupportsIPv4)
        {
            return null;
        }

        string output = string.Empty;

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ipVersion == IpVersion.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                    else if (ipVersion == IpVersion.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }
        return output;
    }
}