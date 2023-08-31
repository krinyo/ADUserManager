using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ADUserManager
{
    internal class RadminButtonStatic : Button
    {
        private string _radminStartCommand = @"C:\Program Files (x86)\Radmin Viewer 3\Radmin.exe";
        private string _pcName;
        public RadminButtonStatic(string content, string pcName, bool withControl)
        {
            Content = content;
            Width = 90;
            _pcName = pcName;

            if (withControl)
                Click += OpenRadminControl;
            else
                Click += OpenRadminNoControl;
        }

        public void OpenRadminControl(object sender, RoutedEventArgs e) 
        {
            var proc = new ProcessStartInfo(@"C:\Program Files (x86)\Radmin Viewer 3\Radmin.exe", "/connect:" + _pcName);
            Process.Start(proc);
        }
        public void OpenRadminNoControl(object sender, RoutedEventArgs e)
        {
            var proc = new ProcessStartInfo(_radminStartCommand, "/connect:" + _pcName + " /noinput");
            Process.Start(proc);
        }
    }
}
