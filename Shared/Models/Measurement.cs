﻿using System.Collections.Generic;

namespace FIT3077.Shared.Models
{
    public class Measurement
    {
        public List<SystolicRecord> SystolicRecords { get; set; }
        public List<DiastolicRecord> DiastolicRecords { get; set; }
        public List<CholesterolRecord> CholesterolRecords { get; set; }

    }
}