using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounterBot
{
    public class Users
    {
        [Key]
        public long User_ID { get; set; }
        public string? Name { get; set; }
        public int Gender { get; set; }
        public int Birthday { get; set; }
    }
}
