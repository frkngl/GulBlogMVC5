using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GülBlogMVC5.Models
{
    public class Data
    {
        public int CurrentCategoryID { get; set; }
        public List<BlogPreviewViewModel> RandomBlogs { get; set; }
        public List<BlogPreviewViewModel> BlogsList { get; set; }
        public List<BlogPreviewViewModel> PopulerBlogs { get; set; }
        public List<CategoryPreviewModels> PopulerCategories { get; set; }
        public List<CategoryPreviewModels> CategoriesList { get; set; }
    }
}