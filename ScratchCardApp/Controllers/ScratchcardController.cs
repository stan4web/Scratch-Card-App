using ScratchCardApp.Models;
using ScratchCardApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Security.Cryptography;
using System.Text;

namespace ScratchCardApp.Controllers
{
    [Authorize]
    public class ScratchcardController : Controller
    {
        private ScratchcardDbContext db = new ScratchcardDbContext();
        //
        // GET: /Scratchcard/

        public ActionResult Index(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "PincodeId desc" : "";

            ViewBag.CurrentSort = sortOrder;

            //var personalData = db.PersonalDatas;
            var pin = from s in db.Pincodes
                               select s;

            ViewBag.Count = db.Pincodes.Count();
            ViewBag.Max = db.Pincodes.Select(m => m.SerialNumber).Max();

            if (!string.IsNullOrEmpty(searchString))
            {
                pin = pin.Where(s => s.PinNumber.ToUpper().Contains(searchString.ToUpper())
                    || s.SerialNumber.ToUpper().Contains(searchString.ToUpper())
                    || s.UserDetails.ToUpper().Contains(searchString.ToUpper())
                    || s.OwnerDetails.ToUpper().Contains(searchString.ToUpper())
                    || s.BatchNumber.ToUpper().Contains(searchString.ToUpper()));

            }
            switch (sortOrder)
            {
                case "CardUsageLogId desc":
                    pin = pin.OrderByDescending(s => s.PinNumber);
                    break;


                default:
                    pin = pin.OrderByDescending(s => s.SerialNumber);
                    break;
            }


            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentFilter = searchString;


            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(pin.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult Card(string id, string batch)
        {
            ViewBag.Success = id;
            ViewBag.Batch = DateTime.Now.Year+ batch;
            ViewBag.Count = db.Pincodes.Count();
          
         return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Card(PinViewModel Pin)
        {
            for (int counter = 1; counter <= Pin.Quantity; counter++)
            {
                int length = Pin.Digit;
                string guidResult = System.Guid.NewGuid().ToString();
                // Remove the hyphens
                guidResult = guidResult.Replace("-", string.Empty);

                // Make sure length is valid
                if (length <= 0 || length > guidResult.Length)
                {
                    throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
                }
                // Return the first length bytes
                ViewBag.Number = guidResult.Substring(1, length).ToUpper();

               
                
                //*********************************
                int limit = 1000000;
                string SerialNo;

                var maxValue = "";

                if (Pin.StartFrom == null)
                {
                    maxValue = db.Pincodes.OrderByDescending(x => x.PincodeId).Where(x => x.SerialNumber.Contains(Pin.SerialCode.ToUpper())).FirstOrDefault().SerialNumber;
                }
                else
                {
                    maxValue = Pin.SerialCode.ToUpper() + "/" + Pin.StartFrom;
                }
                var defaultnumber = Pin.SerialCode.ToUpper() + "/" + "000000";
                if (maxValue == null)
                {
                    maxValue = defaultnumber;
                }
                maxValue = maxValue.Substring(5, 6);
                int rst = int.Parse(maxValue);
                int NewNumber = limit + rst + 1;
                string val = NewNumber.ToString();
                val = val.Substring(1, 6);
                SerialNo = Pin.SerialCode.ToUpper() + "/" + val;
                //SerialNo = "SEMB/" + val;
                Pin.StartFrom = null;
                var year = DateTime.Now.Year;
                //**************************




                Pincode cardNumber = db.Pincodes.Create();
                cardNumber.PinNumber = guidResult.Substring(0, length).ToUpper();
                cardNumber.SerialNumber = SerialNo;
                cardNumber.BatchNumber = year.ToString() + Pin.BatchNumber.ToUpper();
                cardNumber.IsDisenabled = false;
                cardNumber.DateCreated = DateTime.Now;
                cardNumber.UserDetails = User.Identity.Name;
                cardNumber.Usage = 3;
                db.Pincodes.Add(cardNumber);
                db.SaveChanges();
                ViewBag.Success = Pin.Quantity;
                TempData["Check"] = "Ok";
            }
            return RedirectToAction("Card", new { id = ViewBag.Success, batch = Pin.BatchNumber.ToUpper() });
        }

        public ActionResult Search()
        {
            return View();
        }

        public JsonResult AutoCompleteCountry(string term)
        {
            var result = (from r in db.Pincodes
                          where r.PinNumber.ToLower().Contains(term.ToLower())
                          select new { r.PinNumber }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //[Authorize]
        public ActionResult PayCard(string id, string batch)
        {
            ViewBag.Success = id;
            ViewBag.Batch = DateTime.Now.Year + batch;
            ViewBag.Count = db.OnlinePayments.Count();

            return View();
        }
       // [Authorize]
        [HttpPost]
        public ActionResult PayCard(PinViewModel Pin)
        {
            var verifyCon = db.OnlinePayments.ToList();
            if (verifyCon.Count() < 1)
            {
                Pin.StartFrom = "000000";
            }

            for (int counter = 1; counter <= Pin.Quantity; counter++)
            {
                int length = Pin.Digit;

                //--------------Payment Reference Number-----------------------------
                int maxSize = Pin.Digit;
                char[] chars = new char[62];
                string numbers = "0123456789";
                chars = numbers.ToCharArray();
                int size = maxSize;
                byte[] data = new byte[1];
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                size = maxSize;
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
                StringBuilder result = new StringBuilder(size);
                foreach (byte b in data)
                {
                    result.Append(chars[b % (chars.Length - 1)]);
                }

                //ViewBag.Url = result.ToString();


                //*********************************
                int limit = 1000000;
                string SerialNo;

                var maxValue = "";

                if (Pin.StartFrom == null)
                {
                    maxValue = db.OnlinePayments.OrderByDescending(x => x.OnlinePaymentId).Where(x => x.CardSerialNumber.Contains(Pin.SerialCode.ToUpper())).FirstOrDefault().CardSerialNumber;
                }
                else
                {
                    maxValue = Pin.SerialCode.ToUpper() + "/" + Pin.StartFrom;
                }
                var defaultnumber = Pin.SerialCode.ToUpper() + "/" + "000000";
                if (maxValue == null)
                {
                    maxValue = defaultnumber;
                }
                maxValue = maxValue.Substring(5, 6);
                int rst = int.Parse(maxValue);
                int NewNumber = limit + rst + 1;
                string val = NewNumber.ToString();
                val = val.Substring(1, 6);
                SerialNo = Pin.SerialCode.ToUpper() + "/" + val;
                //SerialNo = "SEMB/" + val;
                Pin.StartFrom = null;
                var year = DateTime.Now.Year;
                //**************************
                OnlinePayment cardNumber = db.OnlinePayments.Create();
                cardNumber.CardPinNumber = result.ToString();
                cardNumber.CardSerialNumber = SerialNo;
                cardNumber.BatchNumber = year.ToString() + Pin.BatchNumber.ToUpper();
                cardNumber.IsUsed = false;
                cardNumber.IsValid = true;
                cardNumber.DateProduced = DateTime.Now;
                db.OnlinePayments.Add(cardNumber);
                db.SaveChanges();
                ViewBag.Success = Pin.Quantity;
                TempData["Check"] = "Ok";
            }
            return RedirectToAction("PayCard", new { id = ViewBag.Success, batch = Pin.BatchNumber.ToUpper() });
        }


        public ActionResult OnlinePayIndex(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "PincodeId desc" : "";

            ViewBag.CurrentSort = sortOrder;

            //var personalData = db.PersonalDatas;
            var pin = from s in db.OnlinePayments
                      select s;

            ViewBag.Count = db.OnlinePayments.Count();
            ViewBag.Max = db.Pincodes.Select(m => m.SerialNumber).Max();

            if (!string.IsNullOrEmpty(searchString))
            {
                pin = pin.Where(s => s.CardPinNumber.ToUpper().Contains(searchString.ToUpper())
                    || s.CardSerialNumber.ToUpper().Contains(searchString.ToUpper())
                    || s.BatchNumber.ToUpper().Contains(searchString.ToUpper()));

            }
            switch (sortOrder)
            {
                case "CardUsageLogId desc":
                    pin = pin.OrderByDescending(s => s.CardPinNumber);
                    break;


                default:
                    pin = pin.OrderByDescending(s => s.CardSerialNumber);
                    break;
            }


            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentFilter = searchString;


            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(pin.ToPagedList(pageNumber, pageSize));
        }



       // [Authorize]
        public ActionResult PayCard2(string id, string batch)
        {
            ViewBag.Success = id;
            ViewBag.Batch = DateTime.Now.Year + batch;
            ViewBag.Count = db.PinCodeModels.Count();

            return View();
        }
       // [Authorize]
        [HttpPost]
        public ActionResult PayCard2(PinViewModel Pin)
        {
            var verifyCon = db.PinCodeModels.ToList();
            if (verifyCon.Count() < 1)
            {
                Pin.StartFrom = "000000";
            }

            for (int counter = 1; counter <= Pin.Quantity; counter++)
            {
                int length = Pin.Digit;

                //--------------Payment Reference Number-----------------------------
                int maxSize = Pin.Digit;
                char[] chars = new char[62];
                string numbers = "0123456789";
                chars = numbers.ToCharArray();
                int size = maxSize;
                byte[] data = new byte[1];
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                size = maxSize;
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
                StringBuilder result = new StringBuilder(size);
                foreach (byte b in data)
                {
                    result.Append(chars[b % (chars.Length - 1)]);
                }

                //ViewBag.Url = result.ToString();


                //*********************************
                int limit = 1000000;
                string SerialNo;

                var maxValue = "";

                if (Pin.StartFrom == null)
                {
                    maxValue = db.PinCodeModels.OrderByDescending(x => x.PinCodeModelId).Where(x => x.SerialNumber.Contains(Pin.SerialCode.ToUpper())).FirstOrDefault().SerialNumber;
                }
                else
                {
                    maxValue = Pin.SerialCode.ToUpper() + "/" + Pin.StartFrom;
                }
                var defaultnumber = Pin.SerialCode.ToUpper() + "/" + "000000";
                if (maxValue == null)
                {
                    maxValue = defaultnumber;
                }
                maxValue = maxValue.Substring(5, 6);
                int rst = int.Parse(maxValue);
                int NewNumber = limit + rst + 1;
                string val = NewNumber.ToString();
                val = val.Substring(1, 6);
                SerialNo = Pin.SerialCode.ToUpper() + "/" + val;
                //SerialNo = "SEMB/" + val;
                Pin.StartFrom = null;
                var year = DateTime.Now.Year;
                //**************************
                PinCodeModel cardNumber = db.PinCodeModels.Create();
                cardNumber.PinNumber = result.ToString();
                cardNumber.SerialNumber = SerialNo;
                cardNumber.BatchNumber = year.ToString() + Pin.BatchNumber.ToUpper();
                cardNumber.Usage = 4;
                
                db.PinCodeModels.Add(cardNumber);
                db.SaveChanges();
                ViewBag.Success = Pin.Quantity;
                TempData["Check"] = "Ok";
            }
            return RedirectToAction("PayCard2", new { id = ViewBag.Success, batch = Pin.BatchNumber.ToUpper() });
        }





        public ActionResult Index2(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "PincodeId desc" : "";

            ViewBag.CurrentSort = sortOrder;

            //var personalData = db.PersonalDatas;
            var pin = from s in db.PinCodeModels
                      select s;

            ViewBag.Count = db.PinCodeModels.Count();
            ViewBag.Max = db.Pincodes.Select(m => m.SerialNumber).Max();

            if (!string.IsNullOrEmpty(searchString))
            {
                pin = pin.Where(s => s.PinNumber.ToUpper().Contains(searchString.ToUpper())
                    || s.SerialNumber.ToUpper().Contains(searchString.ToUpper())
                  
                    || s.BatchNumber.ToUpper().Contains(searchString.ToUpper()));

            }
            switch (sortOrder)
            {
                case "CardUsageLogId desc":
                    pin = pin.OrderByDescending(s => s.PinNumber);
                    break;


                default:
                    pin = pin.OrderByDescending(s => s.SerialNumber);
                    break;
            }


            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentFilter = searchString;


            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(pin.ToPagedList(pageNumber, pageSize));
        }

    }
}
