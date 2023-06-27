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

        public MainWindow()
        {
            InitializeComponent();

            manager.SetPrefix("OU=SibirCentrUsers,");
            manager.SetDirectoryEntryPath();
            manager.PropertiesToLoad = new string[]
                {   "cn", "sAMAccountName",
                    "distinguishedName", "telephoneNumber",
                    "mobile", "title", "department", "info" };
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
                    ResponseListBox.Items.Add(new RadminButton("Radmin Control", user, true));
                    ResponseListBox.Items.Add(new RadminButton("Radmin No Control", user, false));
                }
            }
        }
    }
}
