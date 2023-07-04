using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ADUserManager
{
    class OpenInExplorerButton : Button
    {
        private ADUser _user;
        private string _resource;
        public OpenInExplorerButton(string content, ADUser user, string resource) 
        {
            _user = user;
            _resource = resource;
            Content = content;
            Width = 160;
            Click += OpenInExplorer;
        }
        public void OpenInExplorer(object sender, RoutedEventArgs e) 
        {
            if (_user.GetField("info") == null)
                return;
            string path = string.Format("\\\\{0}\\{1}", _user.GetField("info").Split(' ')[4], _resource);
            var proc = new Process();
            //proc.StartInfo.Verb = "runas";
            proc.StartInfo.FileName = "explorer";
            proc.StartInfo.Arguments = path;
            
            proc.Start();
        }
    }
}
