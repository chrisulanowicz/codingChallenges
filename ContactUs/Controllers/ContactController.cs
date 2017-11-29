using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using ContactUs.Models;

namespace ContactUs.Controllers
{

    public class ContactController : Controller
    {

        private ContactContext _context;

        public ContactController(ContactContext context)
        {
            _context = context;
        }

        // method to create List of selectable times for 'Best Time to Call' dropdown option list
        public List<SelectListItem> GetTimeIntervals()
        {
            List<SelectListItem> TimeIntervals = new List<SelectListItem>();
            TimeSpan EarliestTime = new TimeSpan(9,0,0);
            TimeSpan LatestTime = new TimeSpan(18,15,0);
            TimeSpan Interval = new TimeSpan(0,15,0);

            while( EarliestTime < LatestTime )
            {
                TimeIntervals.Add(new SelectListItem { Text = EarliestTime.ToString(@"hh\:mm"), Value = EarliestTime.ToString() });
                EarliestTime = EarliestTime.Add(Interval);
            }

            return TimeIntervals;
        }
        
        [HttpGet]
        [Route("Contact/GetImage")]
        public FileStreamResult GetImage()
        {
            int width = 200;
            int height = 60;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
            
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {

            // This will only get assigned when coming back from a successful form submission in the Create method
            ViewBag.Success = TempData["Success"];

            ViewBag.BestTimes = GetTimeIntervals();
            return View();
        }

        [HttpPost]
        [Route("contact")]
        public IActionResult Create(ContactViewModel model, string userCaptchaCode)
        {

            // Check Captcha
            if(userCaptchaCode != HttpContext.Session.GetString("CaptchaCode"))
            {
                ModelState.AddModelError("Captcha", "Captcha is Incorrect");
            }

            
            if(ModelState.IsValid)
            {
                Contact NewContact = new Contact
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    Telephone = model.Telephone,
                    BestTimeToCall = DateTime.Parse(model.BestTimeToCall.ToString())
                };
                _context.Contacts.Add(NewContact);
                _context.SaveChanges();
                TempData["Success"] = "Thank you for contacting us, we will follow up as soon as possible";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.BestTimes = GetTimeIntervals();
                return View("Index", model);
            }
        }

    }

}