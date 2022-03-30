using System;
using Application;
using System.Collections.Generic;
using System.Diagnostics;
using Mixer_Controller;
using System.Threading;
using System.IO.Ports;
using System.Text;


namespace testingStuff
{
    class Program
    {
        // Create the serial port with basic settings 
        private static SerialPort port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        //private SerialPort port = new SerialPort();





        [STAThread]
        public static void Main(string[] args)
        {
            
            
            port.DtrEnable = true;
            port.RtsEnable = true;

            // Instatiate this 
            new Program().SerialPortProgram();
        }

        private void SerialPortProgram()
        {
            Console.WriteLine("Incoming Data:");
            // Attach a method to be called when there
            // is data waiting in the port's buffer 
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            // Begin communications 
            port.Open();
            // Enter an application loop to keep this thread alive 
            Console.ReadLine();
        }


        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("test123");

            // Show all the incoming data in the port's buffer
            Console.WriteLine(port.ReadExisting());
        }
    }
}
