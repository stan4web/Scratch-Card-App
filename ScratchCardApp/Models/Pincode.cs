using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScratchCardApp.Models
{
    public class Pincode
    {
        public int PincodeId { get; set; }
        public string PinNumber { get; set; }
        public string SerialNumber { get; set; }
        public int Usage { get; set; }
        public string BatchNumber { get; set; }
        public bool IsDisenabled { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        public string UserDetails { get; set; }
        public string OwnerDetails { get; set; }
        public string Category { get; set; }
    }
}