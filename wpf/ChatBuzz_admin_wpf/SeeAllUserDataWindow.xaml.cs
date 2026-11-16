using ChatBuzz_admin_wpf.DTOs;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChatBuzz_admin_wpf
{
    public partial class SeeAllUserDataWindow : Window
    {
        private UserDTO shownuserprivate;

        public SeeAllUserDataWindow(UserDTO shownuser)
        {
            InitializeComponent();
            shownuserprivate = shownuser;
            LoadUserData();
        }

        private void LoadUserData()
        {
            var shownuser = shownuserprivate;

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(shownuser.ProfilePic, UriKind.Absolute);
                bitmap.EndInit();
                userImage.Source = bitmap;
                profilepicstatusLabel.Content = "";
            }
            catch (Exception)
            {
                profilepicstatusLabel.Content = "Profile picture not found";
            }

            emailLabel.Content = "Email: " + shownuser.Email;
            idLabel.Content = "ID: " + shownuser.Id;
            usernameLabel.Content = "Username: " + shownuser.Username;
            bioTextBox.Text = shownuser.Bio;

            isbannedbutton.Background = shownuser.IsBanned ? Brushes.Red : Brushes.Green;
            isadminbutton.Background = shownuser.IsAdmin ? Brushes.Blue : Brushes.LightGray;
        }

        private async void banunbanbutton(object sender, RoutedEventArgs e)
        {
            bool newState = !shownuserprivate.IsBanned;
            string question = newState
                ? "Are you sure you want to ban this user?"
                : "Are you sure you want to unban this user?";

            if (MessageBox.Show(question, "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                string url = newState
                    ? $"api/Users/Ban/{shownuserprivate.Id}"
                    : $"api/Users/Unban/{shownuserprivate.Id}";

                var response = await ApiClient.Http.PutAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(await response.Content.ReadAsStringAsync(), "Hiba");
                    return;
                }

                shownuserprivate.IsBanned = newState;
                LoadUserData();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Az API nem érhető el. Fut a webszerver?");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba");
            }
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}