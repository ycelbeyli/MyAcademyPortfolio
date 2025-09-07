using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Context;
using Portfolio.Web.Entities;
namespace Portfolio.Web.Controllers
{
    public class AboutController(PortfolioContext context) : Controller
    {
        public IActionResult Index()
        {
            var about = context.Abouts.ToList();
            return View(about);
        }

        [HttpGet]

        public IActionResult CreateAbout()
        {
            return View();
        }

        [HttpPost]

        public IActionResult CreateAbout(About about)
        {
            context.Abouts.Add(about);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAbout(int id)
        {
            var about = context.Abouts.Find(id);
            context.Abouts.Remove(about);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult UpdateAbout(int id)
        {
            var about = context.Abouts.Find(id);
            return View(about);
        }

        [HttpPost]
        public IActionResult UpdateAbout(About about)
        {
            context.Update(about);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
