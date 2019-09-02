using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ScratchCardApp.Models
{
    public class ScratchcardDbContext:DbContext
    {
        public DbSet<Pincode> Pincodes { get; set; }
        public DbSet<OnlinePayment> OnlinePayments { get; set; }
        public DbSet<PinCodeModel> PinCodeModels { get; set; }
    }
}