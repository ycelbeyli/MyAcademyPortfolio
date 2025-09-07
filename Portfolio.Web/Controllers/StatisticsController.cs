using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Web.Context;

namespace Portfolio.Web.Controllers
{
    public class StatisticsController(PortfolioContext context) : Controller
    {
        
        public IActionResult Index()
        {
            // Proje Sayısı
            ViewBag.projectCount = context.Projects.Count();
            // Yetenek Ortalaması
            ViewBag.skillAverage = context.Skills.Any() ? context.Skills.Average(x => x.Percentage).ToString("00.00") : 0.0.ToString("00.00");
            // Okunmamış Mesaj Sayısı
            ViewBag.unreadMessageCount = context.UserMessages.Count(x => x.IsRead == false);

            //Son Mesajı Gönderen Kişi
            ViewBag.lastMessageOwner = context.UserMessages.OrderByDescending(x => x.UserMessageId).Select(x => x.Name).FirstOrDefault();


            var startYear = context.Experiences.Min(x => x.StartYear);

            ViewBag.experienceYear = DateTime.Now.Year - startYear;

            ViewBag.companyCount = context.Experiences.Select(x => x.Company).Distinct().Count();

            ViewBag.reviewAverage = context.Testimonials.Any() ? context.Testimonials.Average(x => x.Review).ToString("0.0") : "Değerlendirme Yapılmadı";

            ViewBag.reviewHigh = context.Testimonials.OrderByDescending(x => x.Review).Select(x => x.Name).FirstOrDefault();

            ViewBag.totalMessages = context.UserMessages.Count();

            ViewBag.readMessage = context.UserMessages.Count(x => x.IsRead == true);

            ViewBag.lastCompany = context.Experiences.Where(x => x.EndYear=="Devam Ediyor").Select(x => x.Company).FirstOrDefault();

            ViewBag.lastSchool = context.Educations.OrderByDescending(x=>x.EndYear).Select(x=>x.SchoolName).FirstOrDefault();



            return View();
        }
    }
}
