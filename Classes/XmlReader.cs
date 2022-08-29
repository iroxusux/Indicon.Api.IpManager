using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Indicon.Api.IpManager.Classes
{
    internal static class XmlReader
    {
        private static List<VendorOUI> VendorMacRelations = new();

        internal static void ReadVendorFile(string sFilePath)
        {
            XmlDocument oDoc = new XmlDocument();
            oDoc.Load(@"D:\Personal\VisualStudioProjects\Indicon.Api.IpManager\vendorMacs.xml");
            foreach(XmlNode oNode in oDoc.DocumentElement.ChildNodes)
            {
                if(oNode.Attributes == null)
                {
                    continue;
                }
                VendorMacRelations.Add(new VendorOUI() { MAC = oNode.Attributes["mac_prefix"]?.InnerText, Name = oNode.Attributes["vendor_name"]?.InnerText });
            }
        }
        public static string? GetVendorByMac(string sMAC)
        {
            string[] sSubs = sMAC.Split(":");
            if(sSubs.Length != 6)
            {
                return string.Empty;
            }
            string sOctetOne = NetworkManager.ValidateMacOctet(sSubs[0]);
            string sOctetTwo = NetworkManager.ValidateMacOctet(sSubs[1]);
            string sOctetThree = NetworkManager.ValidateMacOctet(sSubs[2]);
            string sMACSearch = $"{sOctetOne}:{sOctetTwo}:{sOctetThree}";
            VendorOUI oVendor = VendorMacRelations.Find(x => x.MAC == sMACSearch);
            if(oVendor != null)
            {
                return oVendor.Name;
            }
            return string.Empty;
        }
    }
    
}
