using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScratchCardApp.Models;
using System.Text;
using System.Net.Mail;

namespace ScratchCardApp.Controllers
{
    public class HomeController : Controller
    {
        private ScratchcardDbContext db = new ScratchcardDbContext();
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        

        public ActionResult About()
        {

            int length = 6;
            string guidResult = System.Guid.NewGuid().ToString();
            // Remove the hyphens
            guidResult = guidResult.Replace("0123456789", string.Empty);
            // Make sure length is valid
            if (length <= 0 || length > guidResult.Length)
            {
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
            }
            // Return the first length bytes
            ViewBag.Number = guidResult.Substring(1, length).ToUpper();

            TempData["result"] = ViewBag.Number;

            return View();
        }

        public ActionResult Contact()
        {
            
            return View();
        }


        public string TellMeDate()
        {
            return DateTime.Today.ToString();
        }

        public string WelcomeMsg(string input)
        {
            if (!String.IsNullOrEmpty(input))
                return "Please welcome " + input + ".";
            else
                return "Please enter your name.";
        }

        [HttpPost]
        public ActionResult Email()
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add("ibekwestanleyc@gmail.com");
            mailMessage.From = new MailAddress("lollia23@yahoo.com");
            mailMessage.Subject = "ASP.NET e-mail test";
            mailMessage.Body = "Hello world,\n\nThis is an ASP.NET test e-mail!";
            SmtpClient smtpClient = new SmtpClient("smtp.your-isp.com");
            smtpClient.Send(mailMessage);
            @TempData["email"]="E-mail sent!";
            return RedirectToAction("About");
        }

    }
}
