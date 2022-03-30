using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mixer_Controller
{
    class FramePacker
    {
        // input data looks like 
        // fader,0,38
        public static DataFrame pack(string data)
        {
            string[] subs = data.Split(",");

            string prefix = subs[0];
            uint id, value;

            try
            {
                id = uint.Parse(subs[1]);
                value = uint.Parse(subs[2]);

            }
            catch (FormatException e)
            {
                // TODO Print a notifitcation message with the error
                //Trace.WriteLine(e.Message);
                //Trace.WriteLine("could not parse serial data");

                id = 0;
                value = 10;
            }

            return new DataFrame(prefix, id, value);
        }
    }
}
