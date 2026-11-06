using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBuzz_admin_wpf.DTOs
{
    public class usersTodatagrid
    {
        public usersTodatagrid(int id, string username, bool is_banned, DateTime created_at)
        {
            this.id = id;
            this.username = username;
            this.is_banned = is_banned;
            this.created_at = created_at;
        }

        public int id { get; set; }
        public string username { get; set; }
        public bool is_banned { get; set; }
        public DateTime created_at { get; set; }
    }
}
