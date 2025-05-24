using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GülBlogMVC5.Models
{
    public class CategoryPreviewModels
    {
        public int CATEGORYID { get; set; }
        public string CATEGORYNAME { get; set; }
        public string CATEGORYSEOURL { get; set; }
        public int BLOGCOUNT { get; set; }
    }
}