using Microsoft.AspNetCore.Mvc;
using Portfolio.Web.Context;
using Portfolio.Web.Entities;
using System.Linq;

namespace Portfolio.Web.Controllers
{
    public class UserMessageController : Controller
    {
        private readonly PortfolioContext context;

        public UserMessageController(PortfolioContext context)
        {
            this.context = context;
        }

        // Mesaj listesi ve filtreleme
        public IActionResult Index(string filter = "all")
        {
            var messages = context.UserMessages.AsQueryable();

            if (filter == "read")
                messages = messages.Where(m => m.IsRead);
            else if (filter == "unread")
                messages = messages.Where(m => !m.IsRead);

            return View(messages.ToList());
        }

        // Mesaj açma ve okundu olarak işaretleme
        public IActionResult OpenMessage(int id)
        {
            var message = context.UserMessages.Find(id);
            if (message == null) return NotFound();

            if (!message.IsRead)
            {
                message.IsRead = true;
                context.SaveChanges();
            }

            return View("MessageDetail", message);
        }

        // Mesajı okunmadı olarak işaretleme
        public IActionResult MarkAsUnread(int id)
        {
            var message = context.UserMessages.Find(id);
            if (message == null) return NotFound();

            message.IsRead = false;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Mesaj silme
        public IActionResult DeleteUserMessage(int id)
        {
            var message = context.UserMessages.Find(id);
            if (message == null) return NotFound();

            context.UserMessages.Remove(message);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
