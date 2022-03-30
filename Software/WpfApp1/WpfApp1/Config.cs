using System;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mixer_Controller
{
    class Config
    {
        public const string ICON = "C:/Stuff/Programming/Visual Studio Projects/WpfApp1/WpfApp1/Main.ico";

        public const string FILE_NAME = "config.xml";


        private static ChannelLookup.MixerChannel[] _channels;
        private static string _comPort;

        /// <summary>
        /// set variables to the configfile values
        /// </summary>
        public static void init()
        {

            _comPort = XMLHandler.readSetting("comport");
            _channels = XMLHandler.readFaders().ToArray();

            Trace.WriteLine(_comPort);

            // print loop
            foreach (ChannelLookup.MixerChannel chan in _channels)
            {
                string output = chan.getId().ToString() + " " + chan.getPrefix() + " " + chan.getApp();
                Trace.WriteLine(output);
            }
        }

        public static string[] getApps()
        {
            List<string> apps = new List<string>();
            foreach (ChannelLookup.MixerChannel chan in _channels)
            {
                apps.Add(chan.getApp().ToString());
            }
            return apps.ToArray();

        }

        internal static string getComPort()
        {
            return _comPort;
        }

        public static ChannelLookup.MixerChannel[] getChannels()
        {
            return _channels;
        }
    }

    //crackhead code, but it works :)
    class XMLHandler
    {
        // read the remaining settings
        public static string readSetting(string key)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;

            using var reader = XmlReader.Create(Config.FILE_NAME, settings);

            reader.ReadToFollowing("settings");

            reader.ReadToFollowing(key);
            //Console.WriteLine(reader.ReadElementContentAsString());
            string returnVal = reader.ReadElementContentAsString();
            reader.Close();

            return returnVal;
        }

        // read the fader assignments
        public static List<ChannelLookup.MixerChannel> readFaders()
        {
            List<ChannelLookup.MixerChannel> list = new List<ChannelLookup.MixerChannel>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;

            using var reader = XmlReader.Create(Config.FILE_NAME, settings);

            string prefix, application;
            uint id;

            reader.ReadToFollowing("mixerchannel");
            do
            {
                reader.MoveToFirstAttribute();
                reader.MoveToAttribute("mixerchannel");
                Console.WriteLine(reader.Name);

                reader.ReadToFollowing("id");
                try
                {
                    id = uint.Parse(reader.ReadElementContentAsString());
                    Console.WriteLine("id " + id);

                }
                catch (FormatException e)
                {
                    Trace.WriteLine(e.Message);
                    id = 9999;
                }

                reader.ReadToFollowing("prefix");
                prefix = reader.ReadElementContentAsString();
                Console.WriteLine("prefix " + prefix);

                reader.ReadToFollowing("application");
                application = reader.ReadElementContentAsString();
                Console.WriteLine("application " + application);

                list.Add(new ChannelLookup.MixerChannel(prefix, id, application));

            } while (reader.ReadToFollowing("mixerchannel"));
            reader.Close();

            return list;
        }
    }
}
