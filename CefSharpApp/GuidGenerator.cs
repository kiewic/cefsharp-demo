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

        public bool ValidateGuid(string text)
        {
            Guid result;
            return Guid.TryParse(text, out result);
        }

        public bool ValidateGuid2(string foo, string text, long bar)
        {
            Guid result;
            Debug.WriteLine("{0} {1} {2}", foo, text, bar);
            return Guid.TryParse(text, out result);
        }
    }
}
