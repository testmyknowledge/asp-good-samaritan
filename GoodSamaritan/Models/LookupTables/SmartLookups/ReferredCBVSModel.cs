﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoodSamaritan.Models.LookupTables
{
    public class ReferredCBVSModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ReferralCBVSID { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Referral CBVS")]
        public string ReferredCBVS { get; set; }

        public List<SmartModel> Smart { get; set; }
    }
}