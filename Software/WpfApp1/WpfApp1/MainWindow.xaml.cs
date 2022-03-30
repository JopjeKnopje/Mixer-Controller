using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Runtime;

using System.IO.Ports;
using System.Diagnostics;
using System.Windows;
using System.Drawing;
using System.ComponentModel;
using Mixer_Controller;




/// in order to use the System.Windows.Forms 
/// https://stackoverflow.com/questions/57908184/using-system-windows-forms-classes-in-a-net-core-3-0-preview9-project
namespace Application
{
    public partial class MainWindow : Window
    {
        private static SerialHandler _serialHandler = new SerialHandler();

        // constructor
        public MainWindow()
        {
            // calls windows method to init component 0_0
            InitializeComponent();

            // hide the window on startup
            WindowState = WindowState.Minimized;
            Hide();

            ResizeMode = ResizeMode.CanMinimize;
            
            Label label = new Label();
            label.Content = "Ain't nothing here kid";
            label.FontSize = 83;
            AddChild(label);

            Title = "Mixer Controller";

            Uri iconUri = new Uri(Config.ICON, UriKind.RelativeOrAbsolute);

            this.Icon = BitmapFrame.Create(iconUri);


            Background = new SolidColorBrush(Colors.Red);

            _serialHandler.open();
        }

        private void colorParty()
        {
            Random random = new Random();

            byte[] rbg = new byte[3];

            random.NextBytes(rbg);

            System.Windows.Media.Color myRgbColor = new System.Windows.Media.Color();
            myRgbColor = System.Windows.Media.Color.FromRgb(rbg[0], rbg[1], rbg[2]);

            Background = new SolidColorBrush(myRgbColor);

        }

        
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();
            if(WindowState == System.Windows.WindowState.Normal || WindowState ==  System.Windows.WindowState.Maximized)
            {
                colorParty();

            }
            base.OnStateChanged(e);
        }

        /// <summary>
        /// gets called by serialhandler when there is new data
        /// </summary>
        /// the reason for using this over a while loop
        /// is that it saves a shit ton of resources
        public static void callback()
        {
            // get the data
            string data = _serialHandler.poll();
            // if there is data
            if (!data.Equals(""))
            {
                // for debugging
                //Trace.WriteLine(data);

                DataFrame dataframe = FramePacker.pack(data);
                //string app = ChannelLookup.lookup(tmp);
                
                // there is more than one "app" when you have more of the same id 
                // assigned to different applications
                string[] apps = ChannelLookup.lookup(dataframe);


                // TODO Differentiate between faders or buttons
                foreach (string app in apps)
                    Trace.WriteLine("app : " + app);

                MixerController.setProgramVolume(apps, dataframe.getValue());
            }
        }

        // gets called when we press the close button
        protected override void OnClosing(CancelEventArgs e)
        {
            Trace.WriteLine("onclosing");
            // close the serial
            _serialHandler.close();

            // "base" is the java "super" equivalent
            base.OnClosing(e);
        }

    }

}

