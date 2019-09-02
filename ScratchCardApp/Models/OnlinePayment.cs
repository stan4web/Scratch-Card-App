using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScratchCardApp.Models
{
    public class OnlinePayment
    {
        public int OnlinePaymentId { get; set; }
        public string CardPinNumber { get; set; }
        public string CardSerialNumber { get; set; }

        public int? SessionId { get; set; }
        //public Session Session { get; set; }

        public int? UserId { get; set; }
        //public UserProfile UserProfile { get; set; }

        public string PaidFor { get; set; }
        public DateTime? PaidDate { get; set; }

        public DateTime? DateProduced { get; set; }

        public bool IsUsed { get; set; }
        public bool IsValid { get; set; }

        public string BatchNumber { get; set; }
        public string ScerialCode { get; set; }
    }
}