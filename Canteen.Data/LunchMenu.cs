﻿using System;
using System.Collections.Generic;

namespace Canteen.DataAccess
{
    public partial class LunchMenu
    {
        public LunchMenu()
        {
            EmployeeLunches = new HashSet<EmployeeLunch>();
            LunchCancellations = new HashSet<LunchCancellation>();
        }

        public int LunchMenuId { get; set; }
        public short Number { get; set; }
        public short Year { get; set; }
        public int? MondayItemId { get; set; }
        public int? TuesdayItemId { get; set; }
        public int? WednesdayItemId { get; set; }
        public int? ThursdayItemId { get; set; }
        public int? FridayItemId { get; set; }

        public virtual Item? FridayItem { get; set; }
        public virtual Item? MondayItem { get; set; }
        public virtual Item? ThursdayItem { get; set; }
        public virtual Item? TuesdayItem { get; set; }
        public virtual Item? WednesdayItem { get; set; }
        public virtual Week Week { get; set; } = null!;
        public virtual ICollection<EmployeeLunch> EmployeeLunches { get; set; }
        public virtual ICollection<LunchCancellation> LunchCancellations { get; set; }
    }
}
