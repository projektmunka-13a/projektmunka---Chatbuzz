using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBuzz_admin_wpf.classes
{
    internal class comments
    {
        public comments(int id, int post_id, int user_id, string content, DateTime created_at)
        {
            this.id = id;
            this.post_id = post_id;
            this.user_id = user_id;
            this.content = content;
            this.created_at = created_at;
        }

        public int id { get; set; }
        public int post_id { get; set; }
        public int user_id { get; set; }
        public string content { get; set; }
        public DateTime created_at { get; set; }
    }
}
