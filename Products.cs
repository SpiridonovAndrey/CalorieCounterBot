using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounterBot
{
    public class Products
    {
        [Key]
        public Guid ProductID { get; set; }
        public string? ProductName { get; set; }
    }
}
