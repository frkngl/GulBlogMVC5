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
            var categories = db.TBLCATEGORY.Where(x => x.STATUS == true).Select(x => new CategoryPreviewModels
                {
                    CATEGORYNAME = x.CATEGORYNAME,
                    CATEGORYSEOURL = x.SLUG,
                    BLOGCOUNT = x.TBLBLOGS.Count(b => b.STATUS == true)
            }).ToList();

            var modelViews = new Data
            {
                CategoriesList = categories
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
                    x.BLOGDES,
                    x.SLUG
                }).ToList();

            return Json(blogs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult useCategory(string slug)
        {
            var title = db.TBLCATEGORY.FirstOrDefault(x => x.SLUG == slug && x.STATUS == true);

            if (title == null)
            {
                return HttpNotFound("Kategori bulunamadı.");
            }

            int kategoriID = title.ID;
            ViewBag.categoryname = title.CATEGORYNAME;

            var categories = db.TBLCATEGORY.Where(x => x.STATUS == true).Select(x => new CategoryPreviewModels
            {
                CATEGORYNAME = x.CATEGORYNAME,
                CATEGORYSEOURL = x.SLUG,
                BLOGCOUNT = x.TBLBLOGS.Count(b => b.STATUS == true)
            }).ToList();

            var modelViews = new Data
            {
                CategoriesList = categories,
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
                    x.BLOGDES,
                    x.SLUG
                }).ToList();

            return Json(blogs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(string slug)
        {
            var blog = db.TBLBLOGS.Where(x => x.STATUS == true && x.SLUG == slug).Select(x => new BlogPreviewViewModel
            {
                BLOGPIC = x.BLOGPIC,
                BLOGTITLE = x.BLOGTITLE,
                CATEGORYNAME = x.TBLCATEGORY.CATEGORYNAME,
                CATEGORYSEOURL = x.TBLCATEGORY.SLUG,
                NAMEANDSURNAME = x.TBLUSERS.NAMEANDSURNAME,
                DATE = x.DATE,
                PICTURE = x.TBLUSERS.PICTURE,
                BLOGDES = x.BLOGDES,
                FACEBOOK = x.TBLUSERS.FACEBOOK,
                TWITTER = x.TBLUSERS.TWITTER,
                INSTAGRAM = x.TBLUSERS.INSTAGRAM,
                BLOGSEOURL = x.SLUG
            }).FirstOrDefault();

            if (blog == null)
            {
                return HttpNotFound("Blog bulunamadı.");
            }

            ViewBag.blogtitle = blog.BLOGTITLE;

            var modeLViews = new Data
            {
                Blog = blog
            };

            return View(modeLViews);
        }
    }
}