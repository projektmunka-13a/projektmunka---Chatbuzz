using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBuzz_admin_wpf.classes
{
    public class posts
    {
        public posts(int id, int user_id, string username, string content, string image_url, DateTime created_at, List<comments> comments_list, List<post_likes> likes_list)
        {
            this.id = id;
            this.user_id = user_id;
            this.username = username;
            this.content = content;
            this.image_url = image_url;
            this.created_at = created_at;
            this.comments_list = comments_list;
            this.likes_list = likes_list;
        }
        public posts()
        {

        }

        public int id { get; set; }
        public int user_id { get; set; }
        public string username { get; set; }   // a szerző neve (JOIN-ból)
        public string content { get; set; }
        public string image_url { get; set; }
        public DateTime created_at { get; set; }

        public List<comments> comments_list { get; set; } = new List<comments>();
        public List<post_likes> likes_list { get; set; } = new List<post_likes>();

    }
}
