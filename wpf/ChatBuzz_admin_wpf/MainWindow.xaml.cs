using ChatBuzz_admin_wpf.classes;
using ChatBuzz_admin_wpf.DTOs;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ChatBuzz_admin_wpf.classes.posts;

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

        public users usersdatagridselecteditem;

        public MainWindow()
        {
            InitializeComponent();
            usersreadload();
        }

        private  async void usersreadload() 
        {
            try
            {
                string url = "api/Admin/GetUsers";

                var response = await ApiClient.Http.GetAsync(url);
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(body, "Hiba");
                    return;
                }

                var usersList = JsonConvert.DeserializeObject<List<usersTodatagrid>>(body);
                usersdatagrid.ItemsSource = usersList;
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Az API nem érhető el. Fut a webszerver?");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a userek betoltese soran" + ex.Message);
            }
        }

        private async void seeUserdatabuttonclick(object sender, RoutedEventArgs e)
        {
            if (!(usersdatagrid.SelectedItem is usersTodatagrid selected))
            {
                MessageBox.Show("Please select a user from the list to view their data.",
                    "No User Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Application.Current.Windows.OfType<SeeAllUserDataWindow>().Any())
            {
                MessageBox.Show("Már meg van nyitva az ablak");
                return;
            }

            try
            {
                var response = await ApiClient.Http.GetAsync($"api/Users/GetById/{selected.Id}");
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(body, "Hiba");
                    return;
                }

                var fullUser = JsonConvert.DeserializeObject<UserDTO>(body);
                new SeeAllUserDataWindow(fullUser).Show();
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

        private void usersdatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(usersdatagrid.SelectedItem is usersTodatagrid selectedUser)
            {
                var user = fulluserslist.FirstOrDefault(u => u.id == selectedUser.Id);
                usersdatagridselecteditem = user;
                if (user != null)
                {
                    if (user.is_banned)
                    {
                        unbanuserbutton.IsEnabled = true;
                        unbanuserbutton.Foreground = Brushes.Green;
                        banuserbutton.IsEnabled = false;
                        banuserbutton.Foreground = Brushes.Gray;

                    }
                    else
                    {
                        banuserbutton.IsEnabled = true;
                        banuserbutton.Foreground = Brushes.Red;
                        unbanuserbutton.IsEnabled = false;
                        unbanuserbutton.Foreground = Brushes.Gray;
                    }

                }
            }
        }

        private void banuserbuttonclick(object sender, RoutedEventArgs e)
        {
            var selectedindex = usersdatagrid.SelectedIndex;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to BAN this user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                conn.Open();
                var command = new MySqlCommand("UPDATE users SET is_banned = 1 WHERE id = @id", conn);
                command.Parameters.AddWithValue("@id", usersdatagridselecteditem.id);
                command.ExecuteNonQuery();
                conn.Close();
                usersreadload();
                usersdatagrid.Items.Refresh();
                usersdatagrid.SelectedIndex = selectedindex;
            }
        }

        private void unbanuserbuttonclick(object sender, RoutedEventArgs e)
        {
            var selectedindex = usersdatagrid.SelectedIndex;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to UNBAN this user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                conn.Open();
                var command = new MySqlCommand("UPDATE users SET is_banned = 0 WHERE id = @id", conn);
                command.Parameters.AddWithValue("@id", usersdatagridselecteditem.id);
                command.ExecuteNonQuery();
                conn.Close();
                usersreadload();
                usersdatagrid.Items.Refresh();
                usersdatagrid.SelectedIndex = selectedindex;
            }
        }

        private void deleteuserbuttonclick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to DELETE this user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                conn.Open();
                var command = new MySqlCommand("DELETE from users WHERE id = @id", conn);
                command.Parameters.AddWithValue("@id", usersdatagridselecteditem.id);
                command.ExecuteNonQuery();
                conn.Close();
                usersreadload();
                usersdatagrid.Items.Refresh();
            }
        }

        private void showusersactivitybuttonclick(object sender, RoutedEventArgs e)
        {
        
            if (usersdatagridselecteditem == null)
            {
                MessageBox.Show("Please select a user first.");
                return;
            }

            try
            {
                var userPosts = LoadPosts().Where(p => p.user_id == usersdatagridselecteditem.id).ToList();
                postsdatagrid.ItemsSource = userPosts;   // List<posts>

                var userComments = LoadComments(usersdatagridselecteditem.id);
                commentsdatagrid.ItemsSource = userComments;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        


        public List<posts> LoadPosts()
        {
            var result = new List<posts>();
            var byId = new Dictionary<int, posts>();

            using (var conn = new MySqlConnection("server=localhost; database=vizsgaremek_ab_v1; uid=root; password=''; charset=utf8"))
            {
                conn.Open();

                // 1) posztok + szerző neve
                using (var cmd = new MySqlCommand(
                    @"SELECT p.id, p.user_id, p.content, p.image_url, p.created_at, u.username
              FROM posts p JOIN users u ON u.id = p.user_id
              ORDER BY p.created_at DESC", conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var p = new posts
                        {
                            id = r.GetInt32("id"),
                            user_id = r.GetInt32("user_id"),
                            username = r["username"].ToString(),
                            content = r["content"].ToString(),
                            image_url = r["image_url"].ToString(),   // NULL-biztos
                            created_at = r.GetDateTime("created_at")
                        };
                        result.Add(p);
                        byId[p.id] = p;
                    }
                }

                // 2) kommentek + szerző neve
                using (var cmd = new MySqlCommand(@"SELECT c.id, c.post_id, c.user_id, c.content, c.created_at, u.username FROM comments c JOIN users u ON u.id = c.user_id ORDER BY c.created_at", conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int postId = r.GetInt32("post_id");
                        if (!byId.ContainsKey(postId)) continue;

                        byId[postId].comments_list.Add(new comments
                        {
                            id = r.GetInt32("id"),
                            post_id = postId,
                            user_id = r.GetInt32("user_id"),
                            username = r["username"].ToString(),
                            content = r["content"].ToString(),
                            created_at = r.GetDateTime("created_at")
                        });
                    }
                }

                // 3) lájkok + lájkoló neve
                using (var cmd = new MySqlCommand(@"SELECT l.id, l.post_id, l.user_id, l.created_at, u.username FROM post_likes l JOIN users u ON u.id = l.user_id", conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int postId = r.GetInt32("post_id");
                        if (!byId.ContainsKey(postId)) continue;

                        byId[postId].likes_list.Add(new post_likes
                        {
                            id = r.GetInt32("id"),
                            post_id = postId,
                            user_id = r.GetInt32("user_id"),
                            username = r["username"].ToString(),
                            created_at = r.GetDateTime("created_at")
                        });
                    }
                }
            }

            return result;
        }

        public List<comments> LoadComments(int userId)
        {
            var personscomments = new List<comments>();

            using (var conn = new MySqlConnection("server=localhost; database=vizsgaremek_ab_v1; uid=root; password=''; charset=utf8"))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
     @"SELECT comments.id, comments.post_id, comments.user_id, comments.content, comments.created_at, users.username FROM comments JOIN users ON users.id = comments.user_id WHERE comments.user_id = @uid ORDER BY comments.created_at DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            personscomments.Add(new comments
                            {
                                id = r.GetInt32("id"),
                                post_id = r.GetInt32("post_id"),
                                user_id = r.GetInt32("user_id"),
                                username = r["username"].ToString(),
                                content = r["content"].ToString(),
                                created_at = r.GetDateTime("created_at")
                            });
                        }
                    }
                }
            }
            return personscomments;
        }
    }
}
