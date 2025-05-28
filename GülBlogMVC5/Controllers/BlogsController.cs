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
                    x.SLUG,
                    x.VIEWS
                }).ToList();

            var totalCount = db.TBLBLOGS.Count(x => x.STATUS == true);

            return Json(new { blogs, totalCount }, JsonRequestBehavior.AllowGet);
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

            var query = db.TBLBLOGS.Where(x => x.STATUS == true);

            if (kategoriID.HasValue)
            {
                query = query.Where(x => x.CATEGORYID == kategoriID.Value);
            }

            var totalCount = query.Count(); // Toplam aktif blog (filtreye göre)

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
                    x.SLUG,
                    x.VIEWS
                }).ToList();

            return Json(new { blogs, totalCount }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(string slug)
        {
            db.Database.ExecuteSqlCommand("UPDATE TBLBLOGS SET VIEWS = VIEWS + 1 WHERE SLUG = @p0", slug);

            var blogEntity = db.TBLBLOGS .AsNoTracking().FirstOrDefault(x => x.STATUS == true && x.SLUG == slug);

            if (blogEntity == null)
            {
                return HttpNotFound("Blog bulunamadı.");
            }

            var previousPost = db.TBLBLOGS.AsNoTracking().Where(x => x.STATUS == true && x.ID < blogEntity.ID).FirstOrDefault();
            var nextPost = db.TBLBLOGS.AsNoTracking().Where(x => x.STATUS == true && x.ID > blogEntity.ID).FirstOrDefault();

            var blog = new BlogPreviewViewModel
            {
                BLOGPIC = blogEntity.BLOGPIC,
                BLOGTITLE = blogEntity.BLOGTITLE,
                CATEGORYNAME = blogEntity.TBLCATEGORY.CATEGORYNAME,
                CATEGORYSEOURL = blogEntity.TBLCATEGORY.SLUG,
                NAMEANDSURNAME = blogEntity.TBLUSERS.NAMEANDSURNAME,
                DATE = blogEntity.DATE,
                PICTURE = blogEntity.TBLUSERS.PICTURE,
                BLOGDES = blogEntity.BLOGDES,
                FACEBOOK = blogEntity.TBLUSERS.FACEBOOK,
                TWITTER = blogEntity.TBLUSERS.TWITTER,
                INSTAGRAM = blogEntity.TBLUSERS.INSTAGRAM,
                BLOGSEOURL = blogEntity.SLUG,
                VIEWS = blogEntity.VIEWS ?? 0
            };

            var categories = db.TBLCATEGORY.Where(x=>x.STATUS == true).Select(x=>new CategoryPreviewModels
            {
                CATEGORYNAME = x.CATEGORYNAME,
                CATEGORYSEOURL = x.SLUG,
                BLOGCOUNT = x.TBLBLOGS.Count(b => b.STATUS == true)
            }).ToList();

            ViewBag.blogtitle = blog.BLOGTITLE;
            ViewBag.previousPost = previousPost;
            ViewBag.nextPost = nextPost;

            var modeLViews = new Data
            {
                Blog = blog,
                CategoriesList = categories
            };

            return View(modeLViews);
        }

        public JsonResult Search (string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var result = db.TBLBLOGS.Where(x => x.STATUS == true && x.BLOGTITLE.Contains(query))
                .OrderByDescending(x => x.ID)
                .Take(10)
                .Select(x => new
                {
                    title = x.BLOGTITLE,
                    slug = x.SLUG,
                    picture = x.BLOGPIC
                }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}