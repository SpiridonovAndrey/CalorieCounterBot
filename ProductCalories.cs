using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounterBot
{
    public class ProductCalories
    {
        [Key]
        public Guid ProductCaloriesID { get; set; }
        public Guid ProductID { get; set; }
        public decimal Calories { get; set; }
        public long? UserID { get; set; }
    }
}
