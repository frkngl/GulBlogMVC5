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
            return View(veri);
        }

        public ActionResult Detail()
        {
            return View();
        }
    }
}