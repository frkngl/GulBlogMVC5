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
            if (db == null && veri == null)
            {
                return View();
            }

            var randomBlogs = db.TBLBLOGS.Where(x => x.STATUS == true).OrderBy(x=>Guid.NewGuid()).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                CATEGORYNAME = x.TBLCATEGORY.CATEGORYNAME,
                BLOGTITLE = x.BLOGTITLE,
                DATE = x.DATE
            }).Take(10).ToList();

            var blogsList = db.TBLBLOGS.Where(x => x.STATUS == true).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                NAMEANDSURNAME = x.TBLUSERS.NAMEANDSURNAME,
                DATE = x.DATE,
                BLOGTITLE = x.BLOGTITLE,
                CATEGORYNAME = x.TBLCATEGORY.CATEGORYNAME,
                BLOGDES = x.BLOGDES
            }).Take(10).ToList();

            var populerBlog = db.TBLBLOGS.Where(x => x.STATUS == true).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                NAMEANDSURNAME = x.TBLUSERS.NAMEANDSURNAME,
                DATE = x.DATE,
                BLOGTITLE = x.BLOGTITLE
            }).Take(5).ToList();

            var populerCategory = db.TBLCATEGORY.Where(x=>x.STATUS == true).Select(x => new CategoryPreviewModels
            {
                CATEGORYNAME = x.CATEGORYNAME,
                BLOGCOUNT = db.TBLBLOGS.Count(b=>b.CATEGORYID == x.ID && b.STATUS == true)
            }).OrderByDescending(x=>x.BLOGCOUNT).Take(6)
            .ToList();

            var ViewModels = new Data
            {
                RandomBlogs = randomBlogs,
                BlogsList = blogsList,
                PopulerBlogs = populerBlog,
                PopulerCategories = populerCategory
            };

            return View(ViewModels);
        }
    }
}