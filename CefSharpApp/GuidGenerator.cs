using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpApp
{
    public class GuidGenerator
    {
        public string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// This is a sync method that takes 7 seconds.
        /// </summary>
        public bool ValidateGuid(string text)
        {
            //await Task.Delay(7000).ConfigureAwait(true);
            System.Threading.Thread.Sleep(7000);
            Guid result;
            return Guid.TryParse(text, out result);
        }
    }
}
