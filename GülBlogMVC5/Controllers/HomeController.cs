using GülBlogMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            veri.Blog = db.TBLBLOGS.Where(x=>x.STATUS == true).ToList();
            veri.Category = db.TBLCATEGORY.Where(x=>x.STATUS == true).ToList();
            return View(veri);
        }
    }
}