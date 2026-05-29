using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace onTrack
{
    public class AFKTracker
    {
        [DllImport("user32.dll", SetLastError = false)]
        private static extern bool GetLastInputInfo(ref Lastinputinfo plii);
        private static readonly DateTime SystemStartup = DateTime.Now.AddMilliseconds(-Environment.TickCount);

        public static bool EvaluateIsAFK(int maxIdleTime)
        {
            Trace.WriteLine($"Idle for: {IdleTime.TotalSeconds}/{maxIdleTime} Seconds");
            return Math.Floor(IdleTime.TotalSeconds) > 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Lastinputinfo
        {
            public uint cbSize;
            public readonly int dwTime;
        }

        public static DateTime LastInput => SystemStartup.AddMilliseconds(LastInputTicks);

        public static TimeSpan IdleTime => DateTime.Now.Subtract(LastInput);

        private static int LastInputTicks
        {
            get
            {
                var lii = new Lastinputinfo { cbSize = (uint)Marshal.SizeOf(typeof(Lastinputinfo)) };
                GetLastInputInfo(ref lii);
                return lii.dwTime;
            }
        }
    }
}