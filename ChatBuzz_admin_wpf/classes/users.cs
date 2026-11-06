using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBuzz_admin_wpf
{
    public class users
    {
        public users(int id, string username, string email, string profile_pic, string bio, bool is_admin, bool is_banned, DateTime created_at)
        {
            this.id = id;
            this.username = username;
            this.email = email;
            this.profile_pic = profile_pic;
            this.bio = bio;
            this.is_admin = is_admin;
            this.is_banned = is_banned;
            this.created_at = created_at;
        }

        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string profile_pic { get; set; }
        public string bio { get; set; } = string.Empty;
        public bool is_admin { get; set; }
        public bool is_banned { get; set; }
        public DateTime created_at { get; set; }



    }
}
