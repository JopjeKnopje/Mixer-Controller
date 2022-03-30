using System;
using System.Collections.Generic;
using System.Text;

namespace Mixer_Controller
{
    /// <summary>
    /// a basic class to describe the dataframe that the arduino sends
    /// </summary>
    class DataFrame
    {
        // whether its a button or fader
        private string _prefix;
        // id; the physical place its in
        private uint _id, _value;

        // [Prefix],[id],[Val]
        public DataFrame(string prefix, uint id, uint value)
        {
            _prefix = prefix;
            _id = id;
            _value = value;

        }

        // getters
        public string getPrefix() { return _prefix; }
        public uint getId() { return _id; }
        public uint getValue() { return _value; }
    }
}
