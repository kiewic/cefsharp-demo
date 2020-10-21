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
    [System.ComponentModel.DesignerCategory("")]
    public partial class Form1 : Form
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 802);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

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

            // Specifying a CachePath is required for persistence of cookies, saving of passwords, etc.
            settings.CachePath = "cache";

            // For Windows 7 and above, best to include relevant app.manifest entries as well
            //Cef.EnableHighDPISupport();

            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");

            // Initialize cef with the provided settings
            Cef.Initialize(settings);

            #region MultiThreadedMessageLoop
            // CEF message loop integrated into the application message loo (Part 1)
            //settings.MultiThreadedMessageLoop = false;

            //// CEF message loop integrated into the application message loo (Part 2)
            //var timer = new Timer();
            //timer.Interval = 16;
            //timer.Tick += Timer_Tick;
            //timer.Start();
            #endregion

            var result = Cef.AddCrossOriginWhitelistEntry("http://local.kiewic.com", "http", "heyhttp.org", allowTargetSubdomains: false);
            Debug.Assert(result == true, "Calling AddCrossOriginWhitelistEntry");

            Cef.GetGlobalRequestContext().RegisterSchemeHandlerFactory(
                "http",
                domainName: "local.kiewic.com",
                factory: new CustomSchemeHandlerFactory());

            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("http://local.kiewic.com/index.html");
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.KeyboardHandler = new DummyKeyboardHandler();
            chromeBrowser.RequestHandler = new DummyRequestHandler();
            chromeBrowser.IsBrowserInitializedChanged += ChromeBrowser_IsBrowserInitializedChanged;

            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);

            chromeBrowser.JavascriptObjectRepository.Register("coolGuidGenerator", new GuidGenerator());
            chromeBrowser.JavascriptObjectRepository.Register("coolGuidGenerator2", new GuidGenerator(), isAsync: true);
            chromeBrowser.JavascriptObjectRepository.Register("boundAsync", new BoundObject(), true);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Cef.DoMessageLoopWork();
        }

        private void ChromeBrowser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var chromeBrowser = sender as ChromiumWebBrowser;
            if (!chromeBrowser.IsBrowserInitialized)
            {
                throw new InvalidOperationException();
            }

            // DevTools can be called from here too
            chromeBrowser.ShowDevTools();
        }

        // Before CefSharp 75
        //private void ChromeBrowser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        //{
        //    var chromeBrowser = sender as ChromiumWebBrowser;
        //    if (!chromeBrowser.IsBrowserInitialized)
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    // DevTools can be called from here too
        //    // chromeBrowser.ShowDevTools();
        //}
    }
}
