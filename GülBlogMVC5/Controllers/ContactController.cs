using GülBlogMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GülBlogMVC5.Controllers
{
    public class ContactController : Controller
    {
        GulBlogMVC5Entities db = new GulBlogMVC5Entities();
        // GET: Contact
        public ActionResult Index()
        {
            var degerler = db.TBLCONTACT.Select(x => new {x.MAP,x.ADDRESS,x.NUMBER,x.EMAIL}).FirstOrDefault();

            ViewBag.map = degerler?.MAP;
            ViewBag.address = degerler?.ADDRESS;
            ViewBag.number = degerler?.NUMBER;
            ViewBag.email = degerler?.EMAIL;

            return View();
        }
    }
}