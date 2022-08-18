using System.Runtime.InteropServices;
using System.Windows;

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
    }
}
