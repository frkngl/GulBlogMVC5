using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GülBlogMVC5.Models
{
    public class Data
    {
        public List<TBLBLOGS> Blog { get; set; }
        public List<TBLCATEGORY> Category { get; set; }
    }
}