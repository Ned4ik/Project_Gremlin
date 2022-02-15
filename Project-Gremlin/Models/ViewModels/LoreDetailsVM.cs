using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Models.ViewModels
{
    public class LoreDetailsVM
    {
        public LoreDetailsVM()
        {
            Lore = new Lore();
        }
        public Lore Lore { get; set; }
    }
}
