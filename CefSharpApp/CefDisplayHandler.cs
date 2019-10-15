using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.Structs;

namespace CefSharpApp
{
    class CefDisplayHandler : IDisplayHandler
    {
        public void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs)
        {
        }

        public bool OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs)
        {
            // Return true to stop the message from being output to the console.
#if DEBUG
            return false;
#else
            return true;
#endif
        }

        public void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, System.Collections.Generic.IList<string> urls)
        {
        }

        public void OnFullscreenModeChange(IWebBrowser browserControl, IBrowser browser, bool fullscreen)
        {
        }

        public void OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs)
        {
        }

        public void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs)
        {
        }

        public bool OnTooltipChanged(IWebBrowser browserControl, ref string text)
        {
            // To handle the display of the tooltip yourself return true. Otherwise, you can optionally modify |text|
            // and then return false to allow the browser to display the tooltip.
            return false;
        }

        public bool OnAutoResize(IWebBrowser chromiumWebBrowser, IBrowser browser, Size newSize)
        {
            // return false for default handling
            return false;
        }

        public void OnLoadingProgressChange(IWebBrowser chromiumWebBrowser, IBrowser browser, double progress)
        {
        }
    }
}
