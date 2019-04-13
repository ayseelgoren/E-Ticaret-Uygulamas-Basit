using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CikolataWebSitesi.Models;

namespace CikolataWebSitesi.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SendMailToUser(mailTip model)
        {
            bool result = false;
            //konu ,mailmesaj
            if (Session["kullanici"]!=null)
            {result = SendEMail(Session["kullanici"].ToString()+" : "+"  Üye Mesajı", model.mesaj);}
            else
            { result = SendEMail(model.mailkonu, model.mesaj);}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool SendEMail(string mailkonu, string emailBody)
        {

            try
            {
                string senderEmail = "kadriye1315@gmail.com";//gönderen mail
                string senderPassword = " kadriye1315";//gönderen pasword

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, "kulaasumeyye@gmail.com", mailkonu, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }


}