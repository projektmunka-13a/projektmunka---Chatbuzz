using ChatBuzz_admin_wpf.DTOs;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;

namespace ChatBuzz_admin_wpf
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                string url = $"api/Auth/Login/{Uri.EscapeDataString(usernameBox.Text)}?password={Uri.EscapeDataString(passwordBox.Password)}";

                var response = await ApiClient.Http.GetAsync(url);
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(body, "Login failed");
                    return;
                }

                var user = JsonConvert.DeserializeObject<UserDTO>(body);

                if (user == null || !user.IsAdmin)
                {
                    MessageBox.Show("Only admins can use this application.");
                    return;
                }
                ApiClient.CurrentUser = user;
                new MainWindow().Show();
                Close();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Az API nem érhető el. Fut a webszerver?");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}