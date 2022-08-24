using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicon.Api.IpManager.Classes
{
    internal class VendorOUI : IEquatable<VendorOUI>, IComparable<VendorOUI>
    {
        public string? MAC;
        public string? Name;
        public bool Equals(VendorOUI? other)
        {
            if (other == null) return false;
            if (MAC == null) return false;
            return MAC.Equals(other.MAC);
        }
        public bool Equals(VendorOUI? oObject1, VendorOUI? oObject2)
        {
            if (oObject1 == null && oObject2 == null) return true;
            else if (oObject1 == null || oObject2 == null) return false;
            else if (oObject1.MAC == oObject2.MAC) return true;
            else return false;
        }
        public int CompareTo(VendorOUI? oOther)
        {
            if (oOther == null) return -1;
            return string.Compare(MAC, oOther.MAC);
        }
        public int CompareTo(VendorOUI? oObject1, VendorOUI? oObject2)
        {
            if (oObject1 == null && oObject2 == null)
            {
                return 0;
            }
            if (oObject1 == null && oObject2 != null)
            {
                return -1;
            }
            if (oObject1 != null && oObject2 == null)
            {
                return 1;
            }
            return string.Compare(oObject1.MAC, oObject2.MAC);
        }
    }
}
