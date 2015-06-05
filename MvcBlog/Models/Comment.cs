using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcBlog.Models
{
    public class Comment
    {
         public Comment(){
            Createdtime=DateTime.Now;
          }   
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime Createdtime { get; set; }
  
        public virtual Post Post { get; set; }
        
    }
}