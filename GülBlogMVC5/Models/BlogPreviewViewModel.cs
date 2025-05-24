using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GülBlogMVC5.Models
{
    public class BlogPreviewViewModel
    {
        public string BLOGTITLE { get; set; }
        public string BLOGDES { get; set; }
        public string BLOGPIC { get; set; }
        public string CATEGORYNAME { get; set; }
        public string NAMEANDSURNAME { get; set; }
        public DateTime? DATE { get; set; }
    }
}