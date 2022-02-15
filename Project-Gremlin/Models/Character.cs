using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string characterDescription{ get; set; }
        public string imageTitle { get; set; }
        public string characterImage { get; set; }
        public string characterHistory { get; set; }
}
}
