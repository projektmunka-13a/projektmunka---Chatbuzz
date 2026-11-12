using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBuzz_admin_wpf.DTOs
{
    public class usersTodatagrid
    {
        public usersTodatagrid(int id, string username, string email, DateTime created_at)
        {
            this.id = id;
            this.username = username;
            this.email = email;
            this.created_at = created_at;
        }

        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }   
        public DateTime created_at { get; set; }
    }
}
