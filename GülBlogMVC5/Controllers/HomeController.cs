using GülBlogMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace GülBlogMVC5.Controllers
{
    public class HomeController : Controller
    {
        GulBlogMVC5Entities db = new GulBlogMVC5Entities();
        Data veri = new Data();
        // GET: Home
        public ActionResult Index()
        {
            var randomBlogs = db.TBLBLOGS.Where(x => x.STATUS == true).OrderBy(x=>Guid.NewGuid()).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                CATEGORYID = x.CATEGORYID ?? 0,
                CATEGORYSEOURL = x.TBLCATEGORY.SLUG,
                BLOGSEOURL = x.SLUG,
                CATEGORYNAME = x.TBLCATEGORY.CATEGORYNAME,
                BLOGTITLE = x.BLOGTITLE,
                DATE = x.DATE
            }).Take(8).ToList();

            var blogsList = db.TBLBLOGS.Where(x => x.STATUS == true).OrderByDescending(x=>x.ID).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                CATEGORYID =x.CATEGORYID ?? 0,
                CATEGORYSEOURL = x.TBLCATEGORY.SLUG,
                BLOGSEOURL = x.SLUG,
                NAMEANDSURNAME = x.TBLUSERS.NAMEANDSURNAME,
                DATE = x.DATE,
                BLOGTITLE = x.BLOGTITLE,
                CATEGORYNAME = x.TBLCATEGORY.CATEGORYNAME,
                BLOGDES = x.BLOGDES
            }).Take(5).ToList();

            var populerBlog = db.TBLBLOGS.Where(x => x.STATUS == true).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                NAMEANDSURNAME = x.TBLUSERS.NAMEANDSURNAME,
                DATE = x.DATE,
                BLOGTITLE = x.BLOGTITLE,
                BLOGSEOURL = x.SLUG,
            }).Take(5).ToList();

            var populerCategories = db.TBLCATEGORY.Where(x => x.STATUS == true).Select(x => new CategoryPreviewModels
            {
                CATEGORYNAME = x.CATEGORYNAME,
                CATEGORYSEOURL = x.SLUG,
                BLOGCOUNT = x.TBLBLOGS.Count(b => b.STATUS == true)
            }).OrderByDescending(x => x.BLOGCOUNT).Take(6).ToList();

            var ViewModels = new Data
            {
                RandomBlogs = randomBlogs,
                BlogsList = blogsList,
                PopulerBlogs = populerBlog,
                PopulerCategories = populerCategories
            };

            return View(ViewModels);
        }
    }
}