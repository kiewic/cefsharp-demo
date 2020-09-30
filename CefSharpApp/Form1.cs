using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace CefSharpApp
{
    public partial class Form1 : Form
    {
        private enum PROCESS_DPI_AWARENESS
        {
            Process_DPI_Unaware = 0,
            Process_System_DPI_Aware = 1,
            Process_Per_Monitor_DPI_Aware = 2
        }

        [DllImport("SHCore.dll", SetLastError = true)]
        private static extern bool SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);

        public ChromiumWebBrowser chromeBrowser;

        public Form1()
        {
            InitializeComponent();
            InitializeChromium();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void InitializeChromium()
        {
            Debug.Assert(SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.Process_System_DPI_Aware) == false);

            CefSettings settings = new CefSettings();

            //CefSharpSettings.ConcurrentTaskExecution = true;

            settings.RemoteDebuggingPort = 8088;

            // Initialize cef with the provided settings
            Cef.Initialize(settings);

            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("file:///./index.html");
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.KeyboardHandler = new DummyKeyboardHandler();
            chromeBrowser.RequestHandler = new DummyRequestHandler();
            chromeBrowser.IsBrowserInitializedChanged += ChromeBrowser_IsBrowserInitializedChanged;

            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);

            chromeBrowser.JavascriptObjectRepository.Register("boundAsync", new BoundObject(), true);
        }

        private async void ChromeBrowser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var chromiumWebBrowser = sender as ChromiumWebBrowser;
            if (!chromiumWebBrowser.IsBrowserInitialized)
            {
                throw new InvalidOperationException();
            }

            // DevTools can be called from here too
            chromeBrowser.ShowDevTools();
        }
    }
}
