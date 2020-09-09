using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using FurryFriendRnR.UI.Models;

namespace FurryFriendRnR.UI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Service()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Blog()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            string emailbody = $"You have received a message from {cvm.Email} ({cvm.Name}). <br><br>Message: {cvm.Message}";

            MailMessage msg = new MailMessage("no-reply@matthewwessley.com", "matthew.wessley@outlook.com", "Email from matthew.wessley@outlook.com - " + cvm.Phone, emailbody);

            msg.CC.Add("somerandomemail@gmail.com");
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            msg.ReplyToList.Add("matthew.wessley@outlook.com");

            SmtpClient client = new SmtpClient("mail.matthewwessley.com");

            client.Credentials = new NetworkCredential("no-reply@matthewwessley.com", "Roxy0309!");
            client.Port = 8889;

            try
            {
                client.Send(msg);
            }
            catch (System.Exception e)
            {
                ViewBag.ErrorMessage = "Sorry, we were unable to send your message. Please try again at a later time.";

                return View(cvm);
            }

            return View("EmailConfirmation", cvm);
        }


    }
}
