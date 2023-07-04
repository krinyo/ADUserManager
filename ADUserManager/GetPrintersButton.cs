using System.Windows;
using System.Windows.Controls;

namespace ADUserManager
{
    class GetPrintersButton : Button
    {
        private ADUser _user;

        public GetPrintersButton(string content, ADUser user)
        {
            Content = content;
            Width = 160;
            _user = user;
            Click += GetPrinterList;
        }

        public void GetPrinterList(object sender, RoutedEventArgs e) 
        {
            string res = PowerShellRunner.ExecutePowerShellCommand
                (string.Format("Get-WmiObject -Class Win32_Printer -ComputerName \"{0}\"", _user.GetField("info").Split(' ')[4]), true);
            MessageBox.Show(res);
            //Process.Start("powershell", "wmic printer list brief");
        }
    }
}
