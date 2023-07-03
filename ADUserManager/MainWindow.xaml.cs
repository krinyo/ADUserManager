using System.Management.Automation;
using System.Windows;
using System.Windows.Controls;


namespace ADUserManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ADUserManagerUnit manager = new ADUserManagerUnit();
        private PowerShell powerShell;//toDO
        public MainWindow()
        {
            InitializeComponent();

            QueryBox.Focus();

            ConfigFileCreator.CreateConfigFileIfNotExists("config.xml");
            manager.LoadConfiguration("config.xml");
            manager.SetDirectoryEntryPath();

            this.Title += string.Format(" [{0}]", manager.GetDirectoryEntryPath());
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ResponseListBox.Items.Clear();
            if (QueryBox.Text.Length > 2)
            {
                manager.Search_users(QueryBox.Text);
                foreach (ADUser user in manager.Users()) 
                {
                    ResponseListBox.Items.Add(user.ToString());

                    var telephoneSearch = new TelephoneDirectorySearcher("Список телефонов.csv");
                    telephoneSearch.Search(user.GetField("cn"));
                    ResponseListBox.Items.Add(telephoneSearch.ToString());

                    if (user.GetField("info") != null)
                    {
                        ResponseListBox.Items.Add(new RadminButton("Radmin Control", user, true));
                        ResponseListBox.Items.Add(new RadminButton("Radmin No Control", user, false));
                    }

                    ResponseListBox.Items.Add("_________________");
                }
            }
        }

    }
}
