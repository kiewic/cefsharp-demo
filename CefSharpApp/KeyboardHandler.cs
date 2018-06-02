using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using System.IO;
using System.Diagnostics;

namespace CefSharpApp
{
    public static class CefSharpExtensions
    {
        public static Keys GetKey(this CefEventFlags flags)
        {
            Keys key = Keys.None;
            if (flags == CefEventFlags.AltDown)
            {
                key = Keys.Alt;
            }
            if (flags == CefEventFlags.ControlDown)
            {
                key = Keys.Control;
            }
            if (flags == CefEventFlags.ShiftDown)
            {
                key = Keys.Shift;
            }
            return key;
        }
    }

    class KeyboardHandler : IKeyboardHandler
    {
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            Keys key = (Keys)windowsKeyCode | modifiers.GetKey();

            switch (key)
            {
                case Keys.F5 | Keys.None:
                    browser.Reload();
                    return true;
                case Keys.F9 | Keys.None:
                case Keys.F12 | Keys.None:
                    browser.ShowDevTools();
                    return true;
            }

            if (key == (Keys.Control | Keys.P))
            {
                var relativePath = String.Format(@".\output-{0}.pdf", Guid.NewGuid().ToString());
                var fullPath = Path.GetFullPath(relativePath);
                Debug.WriteLine(browser.GetType());
                PdfPrintSettings pdfPrintSettings = new PdfPrintSettings();
                pdfPrintSettings.Landscape = true;
                pdfPrintSettings.BackgroundsEnabled = true;
                browser.PrintToPdfAsync(fullPath, pdfPrintSettings).ContinueWith(OnPrintToPdfCompleted, fullPath);
            }

            return false;
        }

        private void OnPrintToPdfCompleted(Task<bool> result, object fullPath)
        {
            Process.Start(fullPath as string);
        }
    }
}
