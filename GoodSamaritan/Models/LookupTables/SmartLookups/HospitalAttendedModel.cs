﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoodSamaritan.Models.LookupTables
{
    public class HospitalAttendedModel
    {
        [Key]
        public string HospitalAttended { get; set; }

        public List<SmartModel> Smart { get; set; }
    }
}