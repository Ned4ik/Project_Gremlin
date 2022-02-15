using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Models
{
    public class MiniHistory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string historyImage { get; set; }
        public string historyBody { get; set; }
    }
}
