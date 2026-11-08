using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBuzz_admin_wpf.classes
{
    public class post_likes
    {
        public int id { get; set; }
        public int post_id { get; set; }
        public int user_id { get; set; }
        public string username { get; set; }   // JOIN-ból
        public DateTime created_at { get; set; }
    }
}
