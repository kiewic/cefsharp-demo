using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
        public ChromiumWebBrowser chromeBrowser;

        // From: https://stackoverflow.com/a/49115675/27211
        internal enum PROCESS_DPI_AWARENESS
        {
            PROCESS_DPI_UNAWARE = 0,
            PROCESS_SYSTEM_DPI_AWARE = 1,
            PROCESS_PER_MONITOR_DPI_AWARE = 2
        }

        [DllImport("SHcore.dll")]
        internal static extern int GetProcessDpiAwareness(IntPtr hWnd, out PROCESS_DPI_AWARENESS value);


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
            CefSettings settings = new CefSettings();
            // For Windows 7 and above, best to include relevant app.manifest entries as well
            //Cef.EnableHighDPISupport();

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

            chromeBrowser.JavascriptObjectRepository.Register("coolGuidGenerator", new GuidGenerator());
            chromeBrowser.JavascriptObjectRepository.Register("coolGuidGenerator2", new GuidGenerator(), isAsync: true);
            chromeBrowser.JavascriptObjectRepository.Register("boundAsync", new BoundObject(), true);

            PROCESS_DPI_AWARENESS awareness;
            GetProcessDpiAwareness(this.Handle, out awareness);
            Debug.WriteLine(awareness);
        }

        private void ChromeBrowser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            var chromeBrowser = sender as ChromiumWebBrowser;
            if (!chromeBrowser.IsBrowserInitialized)
            {
                throw new InvalidOperationException();
            }

            // DevTools can be called from here too
            // chromeBrowser.ShowDevTools();
        }
    }
}
