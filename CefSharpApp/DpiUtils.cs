using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CefSharpApp
{
    // Source: https://stackoverflow.com/a/49115675/27211
    internal enum PROCESS_DPI_AWARENESS
    {
        PROCESS_DPI_UNAWARE = 0,
        PROCESS_SYSTEM_DPI_AWARE = 1,
        PROCESS_PER_MONITOR_DPI_AWARE = 2
    }

    internal enum DPI_AWARENESS : int
    {
        DPI_AWARENESS_INVALID = -1,
        DPI_AWARENESS_UNAWARE = 0,
        DPI_AWARENESS_SYSTEM_AWARE = 1,
        DPI_AWARENESS_PER_MONITOR_AWARE = 2
    }

    class DpiUtils
    {
        [DllImport("SHcore.dll")]
        internal static extern int GetProcessDpiAwareness(IntPtr hWnd, out PROCESS_DPI_AWARENESS value);

        [DllImport("user32.dll")]
        internal static extern uint GetDpiForWindow(IntPtr hWnd);

        // When app.manifest has:
        //     <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true</dpiAware>
        // Values is:
        //     PROCESS_SYSTEM_DPI_AWARE
        //
        // When app.manifest has:
        //     <dpiAware>true/PM</dpiAware>
        // Value is:
        //     PROCESS_PER_MONITOR_DPI_AWARE
        public static void Check()
        {
            var aTimer = new System.Timers.Timer(5000);
            aTimer.Elapsed += new ElapsedEventHandler(CheckOnInterval);
            aTimer.Enabled = true;
        }

        public static void CheckOnInterval(object source, ElapsedEventArgs e)
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                //if (!String.IsNullOrEmpty(process.MainWindowTitle))
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    // Per process
                    PROCESS_DPI_AWARENESS processAwareness;
                    GetProcessDpiAwareness(process.Handle, out processAwareness);

                    uint windowDpi = GetDpiForWindow(process.MainWindowHandle);

                    if (process.ProcessName != "PBIDesktop") {
                        continue;
                    }

                    Console.WriteLine("Process: {0} ID: {1} Window title: {2} {3} {4}",
                        process.ProcessName,
                        process.Id,
                        process.MainWindowTitle,
                        processAwareness,
                        windowDpi);
                }
            }

            //PROCESS_DPI_AWARENESS awareness;
            //GetProcessDpiAwareness(this.Handle, out awareness);
            Debug.WriteLine("Done");
        }
    }
}
