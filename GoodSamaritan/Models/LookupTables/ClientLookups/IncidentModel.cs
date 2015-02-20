﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoodSamaritan.Models.LookupTables
{
    public class IncidentModel
    {
        [Key]
        public int IncidentId { get; set; }
        public string Incident { get; set; }

        public List<ClientModel> Client { get; set; }

    }
}