using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace CefSharpApp
{
    public partial class Form1 : Form
    {
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

            DpiUtils.Check();
        }
    }
}
