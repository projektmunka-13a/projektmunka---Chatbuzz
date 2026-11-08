using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBuzz_admin_wpf.classes
{
    public class comments
    {
        public comments(int id, int post_id, int user_id, string username, string content, DateTime created_at)
        {
            this.id = id;
            this.post_id = post_id;
            this.user_id = user_id;
            this.username = username;
            this.content = content;
            this.created_at = created_at;
        }
        public comments()
        {

        }

        public int id { get; set; }
        public int post_id { get; set; }
        public int user_id { get; set; }
        public string username { get; set; }   // JOIN-ból



        public string content { get; set; }
        public DateTime created_at { get; set; }
    }
}
