using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace MvcBlog.Models
{
     public class Post
     {   
        public Post(){
            Createdtime=DateTime.Now;
            Author = "Admin";
        }       
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        [MinLength(3)]
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public DateTime Createdtime { get; set; }
        public string Author { get; set; }
        public virtual IList<Comment> Comments { get; set; }
       
        public virtual IList<Image> Images { get; set; }
    }
}