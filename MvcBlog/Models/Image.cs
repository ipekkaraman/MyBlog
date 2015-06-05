using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBlog.Models
{
    public class Image
    {
        public int ID { get; set; }
        public String Path { get; set; }

        public virtual Post Post { get; set; }
        
    }
}