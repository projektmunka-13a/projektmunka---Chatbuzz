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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatBuzz_admin_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class users
        {
            public string username { get; set; }
            public string password { get; set; }
            public string email { get; set; }
            public string role { get; set; }
        }   
        public List<users> userslist = new List<users>();
        public MainWindow()
        {
            InitializeComponent();
            


        }


        
    }
}
