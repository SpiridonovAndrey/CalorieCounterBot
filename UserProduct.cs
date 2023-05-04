using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounterBot
{
    public class UserProduct
    {
        [Key]
        public Guid UserProductID { get; set; }
        public long UserID { get; set; }
        public Guid ProductID { get; set; }
        public int Weight { get; set; }
        public DateTime Date { get; set; }
    }
}
