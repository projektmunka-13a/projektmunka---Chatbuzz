using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace ChatBuzz_admin_wpf
{
    /// <summary>
    /// Interaction logic for SeeAllUserDataWindow.xaml
    /// </summary>
    public partial class SeeAllUserDataWindow : Window
    {
        MySqlConnection conn = new MySqlConnection("server= localhost; database= vizsgaremek_ab_v1; uid=root; password=''; charset = utf8");

        private users shownuserprivate;

        public SeeAllUserDataWindow(users shownuser)
        {
            InitializeComponent();

            LoadUserData(shownuser);

        }

        private void LoadUserData(users shownuser)
        {
            shownuserprivate = shownuser;
            try
            {
                var fullFilePath = shownuser.profile_pic;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                userImage.Source = bitmap;

            }
            catch (Exception ex)
            {
                profilepicstatusLabel.Content = "Profile picture not found";
                Console.Write(ex.Message);
            }

            emailLabel.Content += shownuser.email;
            idLabel.Content += shownuser.id.ToString();
            usernameLabel.Content += shownuser.username;

            bioTextBox.Text = shownuser.bio;

            isbannedbutton.Background = shownuser.is_banned ? Brushes.Red : Brushes.Green;
            isadminbutton.Background = shownuser.is_admin ? Brushes.Blue : Brushes.LightGray;

        }

        private void banunbanbutton(object sender, RoutedEventArgs e)
        {
            if (shownuserprivate.is_banned)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to unban this user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    conn.Open();
                    var command = new MySqlCommand("UPDATE users SET is_banned = 0 WHERE id = @id", conn);
                    command.Parameters.AddWithValue("@id", shownuserprivate.id);
                    command.ExecuteNonQuery();
                    conn.Close();
                }

            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to ban this user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    conn.Open();
                    var command = new MySqlCommand("UPDATE users SET is_banned = 1 WHERE id = @id", conn);
                    command.Parameters.AddWithValue("@id", shownuserprivate.id);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
        } 
    }
}
