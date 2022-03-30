using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using Application;

namespace Mixer_Controller
{
    // TODO Close port when USB Cable is disconnected
    public class SerialHandler
    {
        // hacky way to get the instance
        private static SerialHandler _instance;
        // Speaks for itself
        private SerialPort _serialPort;

        // TODO Set the callback when instantiating the class

        public SerialHandler()
        {
            _instance = this;

            _serialPort = new SerialPort();
            // port settings
            // read the com port from the config class
            _serialPort.PortName = Config.getComPort();
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.Parity = Parity.None;

            // we set these things specifically for the Arduino Pro Micro
            _serialPort.DtrEnable = true;
            _serialPort.RtsEnable = true;

            
            // on data receive event run dataReceived
            _serialPort.DataReceived += dataReceived;
        }

        public void open()
        {
            Trace.WriteLine("ran serial.open()");
            // loop through all the existing serialports
            foreach (string name in SerialPort.GetPortNames()) // why? thats dumb as shit
            {
                // if the current port lines up with our selected port; open it
                // TODO add exception when the serial port already is in use
                if (name.Equals(_serialPort.PortName))
                {
                    //if (_serialPort.IsOpen) break; // if the port is already open;
                    //                               // break the loop so we dont get random opens on multiple ports

                    _serialPort.Open();
                    _serialPort.DiscardOutBuffer();
                    _serialPort.DiscardInBuffer();
                    Trace.WriteLine("opened SerialPort " + _serialPort.PortName);
                }
            }
            // if we can't find the port its probably not open
            if (!_serialPort.IsOpen)
            {
                Trace.WriteLine("SerialPort " + _serialPort.PortName + " not available");
            }
        }


        /// <summary>
        /// closes the serial connnection
        /// </summary>
        public void close()
        {
            _serialPort.Close();
            Trace.WriteLine("closed serial port: " + !_serialPort.IsOpen);
        }

        /// <summary>
        /// reset the serial connection
        /// </summary>
        public void reset()
        {
            close();
            _serialPort.PortName = Config.getComPort();
            open();
        }


        /// <summary>
        /// can be called constantly, only returns a value if the value has changed
        /// </summary>
        /// <returns>string</returns>
        public string poll()
        {
            // checks if there is new data 
            if (DataFilter.isNewData())
            {
                // if there is; return that 
                return DataFilter.getNewData();
            }
            else
            {
                // otherwise return nothing
                return "";
            }
        }

        void dataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            // "cast" the literal object sender "as" a SerialPort and assign that to the variable port
            SerialPort port = sender as SerialPort;


            Trace.WriteLine("DataReceived");

            // Read an entire line.
            // so... it checks if there is a carriage return and only then reads it
            DataFilter.update(port.ReadLine());

            // so it looks like the print statement is slow, but its just the output window so the data speed or whatever is fine
            //Console.WriteLine("datareceived: " + line);

            // TODO Make queue instead of 2 variables


            // TODO make the callback dynamic or make the serialhandler class abstract
            // trigger the callback
            MainWindow.callback();
        }



        // hacky way to get the instance
        public static SerialHandler getInstance()
        { return _instance; }



        /// <summary>
        /// used to check for double data
        /// so we can all pretend its not just 2 variables
        /// TODO maybe find another way to do this 
        /// arrays?
        /// </summary>
        private class DataFilter
        {
            private static string _newData, _oldData;
            public static void update(string newValue)
            {
                _oldData = _newData;
                _newData = newValue;
            }

            /// <summary>
            /// check if they are the same 
            /// </summary>
            /// <returns></returns>
            public static bool isNewData()
            {
                return !_newData.Equals(_oldData);
            }

            public static string getNewData() { return _newData; }
        }
    }
}
