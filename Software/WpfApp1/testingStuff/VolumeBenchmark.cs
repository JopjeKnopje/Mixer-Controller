using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mixer_Controller;

namespace testingStuff
{
    class VolumeBenchmark
    {

        // used to store all the processes that have a audio object
        private static List<Process> audioObjList1 = new List<Process>();
        // contains the app with the audioPid list
        private static IDictionary<string, List<Process>> appObjects = new Dictionary<string, List<Process>>();


        static void start(string[] args)
        {
            const string APP = "chrome";
            const int ITERATIONS = 5;

            Process[] procList = Process.GetProcessesByName(APP);

            updateList(procList);

            Timer mainWatch = new Timer("Main");

            long timeSum = 0;


            for (int x = 0; x < ITERATIONS; x++)
            {
                mainWatch.start();

                updateAudioObjectList(APP);

                for (int i = 100; i >= 0; i--)
                {
                    //volumeDing(procList, i);
                    //Console.WriteLine("volume: " + i);
                    //volumeDingGoed(i);
                    doeVolume(APP, i);
                }
                for (int i = 0; i <= 100; i++)
                {
                    //volumeDing(procList, i);
                    //Console.WriteLine("volume: " + i);
                    //volumeDingGoed(i);
                    doeVolume(APP, i);
                }

                long delta = mainWatch.delta();
                timeSum = timeSum + delta;

                Console.WriteLine("delta: " + delta);
            }
            long average = timeSum / ITERATIONS;
            Console.WriteLine("averagetime: " + average);

            Console.ReadLine();
        }

        public static void doeVolume(string app, int volume)
        {
            Process[] processes = appObjects[app].ToArray();

            foreach (Process p in processes)
            {
                //Console.WriteLine(p.Id);

                MixerController.SetApplicationVolume(p.Id, volume);

            }
        }


        /// <summary>
        /// checks if a app has any audio objects
        /// gets run every first touch of a slider.
        /// gets run every first time the volume is adjusted, after its on a cooldown.
        /// </summary>
        /// <param name="app"></param>
        private static void updateAudioObjectList(string app)
        {
            // get all the processes with the a name
            Process[] processes = Process.GetProcessesByName(app);

            // stores all the objects that have a audio object
            List<Process> audioObjList = new List<Process>();

            // check which processes have an audioObject
            foreach (Process p in processes)
            {
                if (MixerController.hasVolumeObject(p.Id))
                {
                    Console.WriteLine(p.Id + " has an audioObject");
                    // add if audioObjList doesnt already contain Process p
                    if (!audioObjList.Contains(p))
                    {
                        // add it
                        audioObjList.Add(p);
                    }
                }
                // if process p doesn't have a volumeObject
                else
                {
                    // check if it is in the list
                    if (audioObjList.Contains(p))
                    {
                        // if so remove it
                        audioObjList.Remove(p);
                    }
                }
            }

            // check if there is already a audioObjList
            if (appObjects.ContainsKey(app))
            {
                // if so update it
                appObjects[app] = audioObjList;
            }
            else
            {
                // add the audioObjList to the lookup table
                appObjects.Add(app, audioObjList);
            }
        }

        private static void updateList(Process[] pArr)
        {
            foreach (Process p in pArr)
            {
                Console.WriteLine(p.Id);
                if (MixerController.hasVolumeObject(p.Id))
                {
                    audioObjList1.Add(p);
                    Console.WriteLine("added " + p.Id);
                }
            }
            Console.WriteLine("listSize: " + audioObjList1.Count);
        }

        private static void volumeDingGoed(int volume)
        {
            foreach (Process p in audioObjList1)
            {
                MixerController.SetApplicationVolume(p.Id, volume);
            }
        }

        private static void volumeDing(Process[] pArr, float volume)
        {
            foreach (Process p in pArr)
            {
                if (MixerController.hasVolumeObject(p.Id))
                {
                    Console.WriteLine(p.ProcessName);
                    Console.WriteLine(p.SessionId);
                    Console.WriteLine(p.Id);
                    MixerController.SetApplicationVolume(p.Id, volume);
                }
            }
        }

    }
}
