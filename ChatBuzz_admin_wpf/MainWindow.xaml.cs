using ChatBuzz_admin_wpf.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatBuzz_admin_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MySqlConnection conn = new MySqlConnection("server= localhost; database= vizsgaremek_ab_v1; uid=root; password=''; charset = utf8");
          
        public List<users> fulluserslist = new List<users>();
        public List<usersTodatagrid> datagriduserslist = new List<usersTodatagrid>();

        public MainWindow()
        {
            InitializeComponent();
            usersreadload();
           

        }



        private void usersreadload() 
        {
            fulluserslist.Clear();
            conn.Open();
                usersdatagrid.Items.Clear();
                var command = new MySqlCommand("SELECT * FROM users", conn).ExecuteReader();
                while (command.Read())
                {
                    var user = new users(
                        command.GetInt32("id"),
                        command.GetString("username"),
                        command.GetString("email"),
                        command["profile_pic"].ToString(),
                        command["bio"].ToString(),
                        command.GetBoolean("is_admin"),
                        command.GetBoolean("is_banned"),
                        command.GetDateTime("created_at")
                    );
                    fulluserslist.Add(user);
                }
            conn.Close();

            foreach (var user in fulluserslist)
            {
                var datagriduser = new usersTodatagrid(
                    user.id,
                    user.username,
                    user.email,
                    user.created_at
                );
                datagriduserslist.Add(datagriduser);
            }
            usersdatagrid.ItemsSource = datagriduserslist;
            usersdatagrid.Items.Refresh();
        }

        private void seeUserdatabuttonclick(object sender, RoutedEventArgs e)
        {
            var currentselecteduser = usersdatagrid.SelectedItem as usersTodatagrid;
            if (currentselecteduser != null && Application.Current.Windows.OfType<SeeAllUserDataWindow>().Count() == 0)
            {
                var selectedUserFromFullList = fulluserslist.FirstOrDefault(u => u.id == currentselecteduser.id) as users;
                var userdatawindow = new SeeAllUserDataWindow(selectedUserFromFullList);
                userdatawindow.Show();
            }
            else if(Application.Current.Windows.OfType<SeeAllUserDataWindow>().Count() == 1)
            {
                MessageBox.Show("Már meg van nyitva az ablak");
            }
            else
            {
                MessageBox.Show("Please select a user from the list to view their data.", "No User Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
