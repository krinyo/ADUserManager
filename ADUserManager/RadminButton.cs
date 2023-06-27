using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ADUserManager
{
    internal class RadminButton : Button
    {
        private ADUser _clicker;
        private string _radminStartCommand = @"C:\Program Files (x86)\Radmin Viewer 3\Radmin.exe";
        public RadminButton(string content, ADUser user, bool withControl)
        {
            Content = content;
            Width = 160;
            
            _clicker = user;
            if (withControl)
                Click += OpenRadminControl;
            else
                Click += OpenRadminNoControl;
        }

        private void OpenRadminNoControl(object sender, RoutedEventArgs e) 
        {
            if (_clicker.GetField("info") != null)
            {
                string pcName = _clicker.GetField("info").Split(' ')[4];
                var proc = new ProcessStartInfo(_radminStartCommand, "/connect:" + pcName + " /noinput");
                Process.Start(proc);
            }
        }
        private void OpenRadminControl(object sender, RoutedEventArgs e)
        {
            if (_clicker.GetField("info") != null)
            {
                string pcName = _clicker.GetField("info").Split(' ')[4];
                var proc = new ProcessStartInfo(_radminStartCommand, "/connect:" + pcName);
                Process.Start(proc);
            }
        }

    }
}
