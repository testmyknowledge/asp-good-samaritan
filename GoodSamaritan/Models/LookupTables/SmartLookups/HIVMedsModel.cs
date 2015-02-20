﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoodSamaritan.Models.LookupTables
{
    public class HIVMedsModel
    {
        [Key]
        public int HIVMedsId { get; set; }
        public string HIVMeds { get; set; }

        public List<SmartModel> Smart { get; set; }
    }
}