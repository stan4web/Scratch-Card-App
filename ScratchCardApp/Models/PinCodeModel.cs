using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScratchCardApp.Models
{

    public class PinCodeModel
    {
        public int PinCodeModelId { get; set; }

        public string PinNumber { get; set; }

        public string SerialNumber { get; set; }

        public int Usage { get; set; }

        public int? EnrollmentId { get; set; }

        public string BatchNumber { get; set; }

        public string StudentPin { get; set; }
    }
}