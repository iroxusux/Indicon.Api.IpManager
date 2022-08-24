using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Indicon.Api.IpManager.Classes
{
    public static class NativeWindowsFunctions
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
            public static implicit operator Point(POINT oPoint)
            {
                return new Point(oPoint.X, oPoint.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(uint destIP, uint srcIP, byte[] macAddress, ref uint macAddressLength);

        public static byte[] GetMacAddress(IPAddress address)
        {
            byte[] mac = new byte[6];
            uint len = (uint)mac.Length;
            byte[] addressBytes = address.GetAddressBytes();
            uint dest = ((uint)addressBytes[3] << 24)
              + ((uint)addressBytes[2] << 16)
              + ((uint)addressBytes[1] << 8)
              + ((uint)addressBytes[0]);
            if (SendARP(dest, 0, mac, ref len) != 0)
            {
                return null;
            }
            return mac;
        }

        public static void SetStartup(string sAppName, bool bEnabled)
        {
            RegistryKey? oReg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if(oReg == null)
            {
                return;
            }
            if (bEnabled)
            {
                oReg.SetValue(sAppName, Application.ExecutablePath);
            }
            else
            {
                oReg.DeleteValue(sAppName, false);
            }
        }

        public static bool StartupStatus(string sAppName)
        {
            RegistryKey ? oReg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            if(oReg == null)
            {
                return false;
            }
            var oVal = oReg.GetValue(sAppName);
            if(oVal == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
