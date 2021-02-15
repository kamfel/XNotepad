using System;
using System.Runtime.InteropServices;

namespace XNotepad.Core.Utilities
{
    public static class IdleTimeDetector
    {
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static TimeSpan? GetIdleTimeInfo()
        {
            LASTINPUTINFO info = new LASTINPUTINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            if (GetLastInputInfo(ref info))
                return TimeSpan.FromMilliseconds(Environment.TickCount - info.dwTime);
            else
                return null;
        }
    }

    internal struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }
}
