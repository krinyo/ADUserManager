using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ADUserManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ADUserManagerUnit manager = new ADUserManagerUnit();
        private ADComputerManagerUnit compManager = new ADComputerManagerUnit();
        public MainWindow()
        {
            InitializeComponent();

            QueryBox.Focus();

            ConfigFileCreator.CreateConfigFileIfNotExists("config.xml");
            manager.LoadConfiguration("config.xml");
            manager.SetDirectoryEntryPath();

            this.Title += string.Format(" [{0}]", manager.GetDirectoryEntryPath());

            foreach (var pc in compManager.GetComputers())
            {
                ComputersListBox.Items.Add(pc.Key);
                ComputersListBox.Items.Add(new RadminButtonStatic("Control", pc.Key, true));
                ComputersListBox.Items.Add(new RadminButtonStatic("No control", pc.Key, false));
                ComputersListBox.Items.Add("______________________");
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ResponseListBox.Items.Clear();
            if (QueryBox.Text.Length > 2)
            {
                manager.SearchUsers(QueryBox.Text);
                foreach (ADUser user in manager.Users()) 
                {
                    ResponseListBox.Items.Add(user.ToString());

                    try
                    {
                        var telephoneSearch = new TelephoneDirectorySearcher("Список телефонов.csv");
                        telephoneSearch.Search(user.GetField("cn"));
                        ResponseListBox.Items.Add(telephoneSearch.ToString());
                    }
                    catch {
                        MessageBox.Show("No telephone file!");
                    }


                    if (user.GetField("info") != null)
                    {
                        ResponseListBox.Items.Add(new RadminButton("Radmin Control", user, true));
                        ResponseListBox.Items.Add(new RadminButton("Radmin No Control", user, false));

                        ResponseListBox.Items.Add(new OpenInExplorerButton("Open c$", user, "c$"));
                        ResponseListBox.Items.Add(new GetPrintersButton("Printers", user));
                    }

                    ResponseListBox.Items.Add("_________________");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(QueryBox.Text != string.Empty)
            {
                string pcName = QueryBox.Text;
                var proc = new ProcessStartInfo(@"C:\Program Files (x86)\Radmin Viewer 3\Radmin.exe", "/connect:" + pcName);
                Process.Start(proc);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (QueryBox.Text != string.Empty)
            {
                string pcName = QueryBox.Text;
                var proc = new ProcessStartInfo(@"C:\Program Files (x86)\Radmin Viewer 3\Radmin.exe", "/connect:" + pcName + " /noinput");
                Process.Start(proc);
            }
        }

        private void ComputersBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            var computers = compManager.GetComputers();
            ComputersListBox.Items.Clear();
            if (ComputersQuery.Text.Length == 0) 
            {
                foreach (var pc in computers)
                {
                    ComputersListBox.Items.Add(pc.Key);
                    ComputersListBox.Items.Add(new RadminButtonStatic("Control", pc.Key, true));
                    ComputersListBox.Items.Add(new RadminButtonStatic("No control", pc.Key, false));
                    ComputersListBox.Items.Add("______________________");
                }
            }
            if (ComputersQuery.Text.Length >= 2) 
            {
                /*if (computers.ContainsKey(ComputersQuery.Text.ToUpper()))
                {
                    ComputersListBox.Items.Add(ComputersQuery.Text.ToUpper());
                    ComputersListBox.Items.Add(new RadminButtonStatic("Control", ComputersQuery.Text.ToUpper(), true));
                    ComputersListBox.Items.Add(new RadminButtonStatic("No control", ComputersQuery.Text.ToUpper(), false));
                    ComputersListBox.Items.Add("______________________");
                }*/
                foreach (var key in computers.Keys) 
                {
                    if (key.ToLower().Contains(ComputersQuery.Text.ToLower())) 
                    {
                        ComputersListBox.Items.Add(key.ToString());
                        ComputersListBox.Items.Add(new RadminButtonStatic("Control", key.ToString(), true));
                        ComputersListBox.Items.Add(new RadminButtonStatic("No control", key.ToString(), false));
                        ComputersListBox.Items.Add("______________________");
                    }
                }
            }
        }
    }
}
