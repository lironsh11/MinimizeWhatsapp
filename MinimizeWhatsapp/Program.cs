using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MinimizeWhatsapp
{
    internal class Program
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll")]

        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }
        [STAThread]
        static void Main(string[] args)
        {
            while (true)
            {
                Task.Delay(30000).Wait();
                if (GetLastInputTime() >60)
                {
                    MinimizeCurrentWindow("WhatsApp");
                }
                else
                {
                }
            }
        }
        static uint GetLastInputTime()
        {
            uint idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;

                idleTime = envTicks - lastInputTick;
            }

            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }

        public static void MinimizeCurrentWindow(string windowTitle)
        {
            const int WM_SYSCOMMAND = 0x0112;
            int SC_MINIMIZE = 0xF020;
            IntPtr window = FindWindow(null, windowTitle);
            if (window != IntPtr.Zero)
            {
                SendMessage((int)window, WM_SYSCOMMAND, SC_MINIMIZE, 0);
            }
        }
       
      
    }
}
