using GülBlogMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GülBlogMVC5.Controllers
{
    public class BlogsController : Controller
    {
        GulBlogMVC5Entities db = new GulBlogMVC5Entities();
        // GET: Blogs
        Data veri = new Data();
        public ActionResult Index()
        {
            var categoryRawList = db.TBLCATEGORY.Where(x => x.STATUS == true).Select(x => new
            {
                x.ID,
                x.CATEGORYNAME,
                BLOGCOUNT = x.TBLBLOGS.Count(b => b.STATUS == true)
                }).ToList();

            var categories = categoryRawList.Select(x => new CategoryPreviewModels
                {
                    CATEGORYID = x.ID,
                    CATEGORYNAME = x.CATEGORYNAME,
                    CATEGORYSEOURL = ToSeoUrl(x.CATEGORYNAME),
                    BLOGCOUNT = x.BLOGCOUNT
                }).ToList();

            var populerBlog = db.TBLBLOGS.Where(x=>x.STATUS == true).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                NAMEANDSURNAME = x.TBLUSERS.NAMEANDSURNAME,
                DATE = x.DATE,
                BLOGTITLE = x.BLOGTITLE
            }).Take(2).ToList();

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
                }).OrderByDescending(x => x.BLOGCOUNT) .Take(6).ToList();

            var modelViews = new Data
            {
                CategoriesList = categories,
                PopulerBlogs = populerBlog,
                PopulerCategories = populerCategories
            };
            return View(modelViews);
        }

        public JsonResult LoadMoreBlogs(int page = 1, int pageSize = 4)
        {
            var skip = (page - 1) * pageSize;

            var blogs = db.TBLBLOGS
                .Where(x => x.STATUS == true)
                .OrderByDescending(x => x.ID)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new
                {
                    x.BLOGPIC,
                    x.TBLUSERS.NAMEANDSURNAME,
                    x.DATE,
                    x.BLOGTITLE,
                    x.TBLCATEGORY.CATEGORYNAME,
                    x.BLOGDES
                }).ToList();

            return Json(blogs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult useCategory(int kategoriID, string kategoriName)
        {

            var categoryRawList = db.TBLCATEGORY.Where(x => x.STATUS == true).Select(x => new
            {
                x.ID,
                x.CATEGORYNAME,
                BLOGCOUNT = x.TBLBLOGS.Count(b => b.STATUS == true)
            }).ToList();

            var categories = categoryRawList.Select(x => new CategoryPreviewModels
            {
                CATEGORYID = x.ID,
                CATEGORYNAME = x.CATEGORYNAME,
                CATEGORYSEOURL = ToSeoUrl(x.CATEGORYNAME),
                BLOGCOUNT = x.BLOGCOUNT
            }).ToList();

            var usekategori = db.TBLCATEGORY.FirstOrDefault(x => x.ID == kategoriID);
            ViewBag.categoryname = usekategori.CATEGORYNAME;

            var populerBlog = db.TBLBLOGS.Where(x => x.STATUS == true).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                NAMEANDSURNAME = x.TBLUSERS.NAMEANDSURNAME,
                DATE = x.DATE,
                BLOGTITLE = x.BLOGTITLE
            }).Take(2).ToList();

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

            var modelViews = new Data
            {
                CategoriesList = categories,
                PopulerBlogs = populerBlog,
                PopulerCategories = populerCategories,
                CurrentCategoryID = kategoriID
            };
            return View(modelViews);
        }


        public JsonResult LoadMoreBlogss(int page = 1, int pageSize = 4, int? kategoriID = null)
        {
            var skip = (page - 1) * pageSize;

            var query = db.TBLBLOGS
                .Where(x => x.STATUS == true);

            if (kategoriID.HasValue)
            {
                query = query.Where(x => x.CATEGORYID == kategoriID.Value);
            }

            var blogs = query
                .OrderByDescending(x => x.ID)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new
                {
                    x.BLOGPIC,
                    x.TBLUSERS.NAMEANDSURNAME,
                    x.DATE,
                    x.BLOGTITLE,
                    x.TBLCATEGORY.CATEGORYNAME,
                    x.BLOGDES
                }).ToList();

            return Json(blogs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail()
        {
            return View();
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