using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace Mixer_Controller
{
    public class Timer
    {
        private static uint _instances = 0;

        private bool _log;
        private string _name;
        private long _newTime, _oldTime, _delta;
        public Timer(string name = "", bool measureOnInit = false, bool log = false)
        {
            if (name.Equals("")) _name = _instances.ToString();
            _name = name;

            if (measureOnInit)
            {
                start();
                Console.WriteLine("measureOnInit");
                Trace.WriteLine("measureOnInit");
            }
            _log = log;

            Console.WriteLine("timer created: " + _name);
            Trace.WriteLine("timer created: " + _name);

            _instances++;
        }

        public void start()
        {
            _oldTime = _getTime();
            if (_log)
            {
                Console.WriteLine("timer" + _name + " start: " + _oldTime);
                Trace.WriteLine("timer" + _name + " start: " + _oldTime);
            }
        }

        public void stop()
        {
            _newTime = _getTime();
            if (_log)
            {
                Console.WriteLine("timer" + _name + " stop: " + _newTime);
                Trace.WriteLine("timer" + _name + " stop: " + _newTime);
            }
        }


        public long delta()
        {
            stop();
            _delta = _newTime - _oldTime;
            if (_log)
            {
                Console.WriteLine("timer" + _name + " delta: " + _delta);
                Trace.WriteLine("timer" + _name + " delta: " + _delta);
            }
            return _delta;
        }

        private long _getTime()
        {
            long tmp = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
            return tmp;
        }
    }
}
