﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoodSamaritan.Models.LookupTables
{
    public class SocialWorkAttendanceModel
    {
        [Key]
        public int SocalWorkAttendanceId { get; set; }
        public string SocialWorkAttendance { get; set; }

        public List<SmartModel> Smart { get; set; }
    }
}