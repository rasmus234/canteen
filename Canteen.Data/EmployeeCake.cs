using System;
using System.Collections.Generic;

namespace Canteen.DataAccess
{
    public partial class EmployeeCake
    {
        public int EmployeeId { get; set; }
        public int ItemId { get; set; }
        public short Number { get; set; }
        public short Year { get; set; }
        public short Limit { get; set; }
        public string Day { get; set; } = null!;

        public virtual Employee Employee { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
        public virtual Week Week { get; set; } = null!;
    }
}
