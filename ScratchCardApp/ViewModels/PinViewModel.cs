using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScratchCardApp.ViewModels
{
    public class PinViewModel
    {
        [Required(ErrorMessage = "Enter the quantity")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Enter the batch number")]
        public string BatchNumber { get; set; }

        public int Digit { get; set; }

        [StringLength(4, ErrorMessage = "The {0} must be {2} digits.", MinimumLength = 4)]
        [Required(ErrorMessage = "Enter the serial code")]
        public string SerialCode { get; set; }

        [StringLength(6, ErrorMessage = "The {0} must be {2} digits.", MinimumLength = 6)]
        //[Required(ErrorMessage = "Enter the starting point")]
        public string StartFrom { get; set; }
    }
}