using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<MiniHistory> MiniHistories { get; set; }
    }
}
