using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Models.ViewModels
{
    public class HistoryDetailsVM
    {
        public HistoryDetailsVM()
        {
            MiniHistory = new MiniHistory();
        }
        public MiniHistory MiniHistory { get; set; }
    }
}
