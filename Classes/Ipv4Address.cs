namespace Indicon.Api.IpManager.Classes
{
    [Serializable]
    public sealed class Ipv4Address : IEquatable<Ipv4Address>, IComparable<Ipv4Address>
    {
        public string? Name { get; private set; }
        public int[] IpAddress { get; private set; } = Array.Empty<int>();
        public string IpAddressString { get { return string.Join(".", IpAddress.Select(x => x.ToString())); } }
        public int[] SubnetMask { get; private set; } = Array.Empty<int>();
        public string SubnetMaskString { get { return string.Join(".", SubnetMask.Select(x => x.ToString())); } }
        public int[] Gateway { get; private set; } = Array.Empty<int>();
        public string GatewayString { get { return string.Join(".", Gateway.Select(x => x.ToString())); } }
        public string? NetworkInterfaceName { get; set; }
        public string? NetworkInterfaceMAC { get; set; } 
        private const int ARRAY_SIZE = 4;
        private const int NETWORK_LOW = 0;
        private const int NETWORK_HIGH = 255;

        public void SetName(string sName)
        {
            Name = sName;
        }
        public void SetIpAddress(string[] sOctets)
        {
            IpAddress = SetNetworkParams(sOctets);
        }
        public void SetSubnetMask(string[] sOctets)
        {
            SubnetMask = SetNetworkParams(sOctets);
        }
        public void SetGateway(string[] sOctets)
        {
            Gateway = SetNetworkParams(sOctets);
        }
        private int[] SetNetworkParams(string[] oSource)
        {
            if(oSource.Length != ARRAY_SIZE)
            {
                throw new InvalidOperationException();
            }
            int[] iAdapter = new int[ARRAY_SIZE];
            for(int i = 0; i < ARRAY_SIZE; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(oSource[i]))
                    {
                        iAdapter[i] = 0;
                        continue;
                    }
                    int iOctet = Convert.ToInt32(oSource[i]);
                    if(iOctet < NETWORK_LOW || iOctet > NETWORK_HIGH)
                    {
                        throw new InvalidOperationException();
                    }
                    iAdapter[i] = iOctet;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException();
                }
            }
            return iAdapter;
        }
        public override string ToString()
        {
            return $"{Name} {string.Join(".", IpAddress)} {string.Join(".", SubnetMask)} {string.Join(".", Gateway)} {NetworkInterfaceName}";
        }
        public bool Equals(Ipv4Address? other)
        {
            if (other == null) return false;
            return Name.Equals(other.Name);
        }
        public bool Equals(Ipv4Address? oObject1, Ipv4Address? oObject2)
        {
            if (oObject1 == null && oObject2 == null) return true;
            else if (oObject1 == null || oObject2 == null) return false;
            else if (oObject1.Name == oObject2.Name) return true;
            else return false;
        }
        public int CompareTo(Ipv4Address? oOther)
        {
            if(oOther == null) return -1;
            return string.Compare(Name, oOther.Name);
        }
        public int CompareTo(Ipv4Address? oObject1, Ipv4Address? oObject2)
        {
            if(oObject1 == null && oObject2 == null)
            {
                return 0;
            }
            if(oObject1 == null && oObject2 != null)
            {
                return -1;
            }
            if(oObject1 != null && oObject2 == null)
            {
                return 1;
            }
            return string.Compare(oObject1.Name, oObject2.Name);
        }
    }
}
