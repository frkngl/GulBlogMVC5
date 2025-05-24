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

            var rawRandomBlogs = db.TBLBLOGS.Where(x => x.STATUS == true).Select(x => new
            {
                x.BLOGPIC,
                x.CATEGORYID,
                x.BLOGTITLE,
                x.TBLCATEGORY.CATEGORYNAME,
                x.DATE
            }).ToList();

            var randomBlogs = rawRandomBlogs.OrderBy(x=>Guid.NewGuid()).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                CATEGORYID = x.CATEGORYID ?? 0,
                CATEGORYSEOURL = ToSeoUrl(x.CATEGORYNAME),
                CATEGORYNAME = x.CATEGORYNAME,
                BLOGTITLE = x.BLOGTITLE,
                DATE = x.DATE
            }).Take(10).ToList();

            var rawBlogList = db.TBLBLOGS.Where(x => x.STATUS == true).Select(x => new
            {
                x.BLOGPIC,
                x.CATEGORYID,
                x.TBLUSERS.NAMEANDSURNAME,
                x.DATE,
                x.BLOGTITLE,
                x.TBLCATEGORY.CATEGORYNAME,
                x.BLOGDES
            }).ToList();

            var blogsList = rawBlogList.Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                CATEGORYID =x.CATEGORYID ?? 0,
                CATEGORYSEOURL = ToSeoUrl(x.CATEGORYNAME),
                NAMEANDSURNAME = x.NAMEANDSURNAME,
                DATE = x.DATE,
                BLOGTITLE = x.BLOGTITLE,
                CATEGORYNAME = x.CATEGORYNAME,
                BLOGDES = x.BLOGDES
            }).Take(10).ToList();

            var populerBlog = db.TBLBLOGS.Where(x => x.STATUS == true).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                NAMEANDSURNAME = x.TBLUSERS.NAMEANDSURNAME,
                DATE = x.DATE,
                BLOGTITLE = x.BLOGTITLE
            }).Take(5).ToList();

            var rawCategories = db.TBLCATEGORY.Where(x => x.STATUS == true).Select(x => new
            {
                x.ID,
                x.CATEGORYNAME,
                BLOGCOUNT = x.TBLBLOGS.Count(b => b.STATUS == true)
            }).ToList();

            var populerCategories = rawCategories.Select(x => new CategoryPreviewModels
            {
                CATEGORYID = x.ID,
                CATEGORYNAME = x.CATEGORYNAME,
                CATEGORYSEOURL = ToSeoUrl(x.CATEGORYNAME),
                BLOGCOUNT = x.BLOGCOUNT
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

        public static string ToSeoUrl(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            var normalized = text.ToLower()
                .Replace("ç", "c")
                .Replace("ğ", "g")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ş", "s")
                .Replace("ü", "u")
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace(":", "")
                .Replace(",", "")
                .Replace(".", "")
                .Replace("?", "")
                .Replace("&", "")
                .Replace("/", "-");

            return normalized;
        }
    }
}