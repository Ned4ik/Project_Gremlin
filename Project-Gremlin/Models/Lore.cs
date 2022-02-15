using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Models
{
    public class Lore
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageTitle { get; set; }
        public string loreBody { get; set; }
    }
}
