using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using Mixer_Controller;

namespace Application
{
    /// App inherits from Application
    public partial class App : System.Windows.Application
    {

        private readonly System.Windows.Forms.NotifyIcon _notifyIcon;

        // constructor   
        public App()
        {
            Trace.WriteLine("app()");

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Trace.WriteLine("on startup");

            _notifyIcon.Icon = new System.Drawing.Icon(Config.ICON);
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Mixer Controller";
            _notifyIcon.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    MainWindow.Show();
                    MainWindow.WindowState = WindowState.Normal;
                    
                    Trace.WriteLine("notifycation icon pressed");
                };
            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, onClick);
            _notifyIcon.ContextMenuStrip.Items.Add("Reload Config", null, onClick);


            Config.init();
            ChannelLookup.init();
            MixerController.init();

            // proceed with normal startup behaviour
            base.OnStartup(e);
        }

        private void onClick(object sender, EventArgs e)
        {
            if(sender.ToString().Equals("Reload Config"))
            {
                Config.init();
                ChannelLookup.init();
                MixerController.init();

                // Also reset the serial connection
                SerialHandler.getInstance().reset();


                Trace.WriteLine("Reloaded config");

                // add updated serialport to the notification message                
                string nofticationMsg = "Serialport: " + Config.getComPort() + "\n";

                // use a IEnumerable to iterate through the list in reverse
                IEnumerable<ChannelLookup.MixerChannel> enumerableThing = Config.getChannels();

                // iterate in reverse
                foreach (ChannelLookup.MixerChannel chan in enumerableThing.Reverse())
                {
                    nofticationMsg += chan.getId().ToString() + " | " + chan.getPrefix().ToString() + " | " + chan.getApp().ToString() + "\n";

                }
                // When reloaded do a textballoon with the updated values
                _notifyIcon.ShowBalloonTip(3000, "Reloaded config", nofticationMsg, System.Windows.Forms.ToolTipIcon.Info);
            }

            if (sender.ToString().Equals("Exit"))
            {
                // when clicked close the MainWindow which will also call our OnExit
                MainWindow.Close();
            }        
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Trace.WriteLine("on exit");
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
