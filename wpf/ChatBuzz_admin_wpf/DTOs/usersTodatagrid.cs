using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBuzz_admin_wpf.DTOs
{
    public class usersTodatagrid
    {
        public usersTodatagrid()
        {
           
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }   
        public DateTime CreatedAt { get; set; }
    }
}
