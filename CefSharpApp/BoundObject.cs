using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpApp
{
    class BoundObject
    {
        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public void Error()
        {
            throw new Exception("This is an exception coming from C#");
        }

        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public int Div(int dividend, int divisor)
        {
            return dividend / divisor;
        }
    }
}
