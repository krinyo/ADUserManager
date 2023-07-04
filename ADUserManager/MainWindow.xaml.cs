using System.Windows;
using System.Windows.Controls;

namespace ADUserManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ADUserManagerUnit manager = new ADUserManagerUnit();
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

    }
}
