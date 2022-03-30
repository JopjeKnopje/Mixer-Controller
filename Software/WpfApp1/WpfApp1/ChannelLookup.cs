using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mixer_Controller
{
    /// <summary>
    /// lookuptable, kinda
    /// </summary>
    class ChannelLookup
    {
        private static MixerChannel[] _channels;

        /// <summary>
        /// loads all the channels from the config class
        /// </summary>
        public static void init()
        {
            _channels = Config.getChannels();


        }


        // spits out the assigned application based on what the dataframe contains
        // public static string lookup(DataFrame frame)
        public static string[] lookup(DataFrame frame)
        {
            List<string> applications = new List<string>();
            uint id = frame.getId();
            string prefix = frame.getPrefix();
            //idk how so lets just iterate through them
            foreach (MixerChannel chan in _channels)
            {
                if (chan.getId() == id)
                    if (chan.getPrefix().Equals(prefix))
                        applications.Add(chan.getApp());
                //return chan.getApp();

            }
            // if there was nothing return empty string
            //return "";
            return applications.ToArray();
        }



        /// <summary>
        /// a storage object for each channel
        /// </summary>
        public class MixerChannel
        {
            private uint _id;
            private string _prefix = "";
            private string _app = "";
            public MixerChannel(string prefix, uint id, string app)
            {
                _prefix = prefix;
                _id = id;
                _app = app;
            }

            public uint getId() { return _id; }
            public string getPrefix() { return _prefix; }
            public string getApp() { return _app; }

        }
    }
}
