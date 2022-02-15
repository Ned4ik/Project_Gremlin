using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Models.ViewModels
{
    public class CharacterDetailsVM
    {
        public CharacterDetailsVM()
        {
            Character = new Character();
        }
        public Character Character { get; set; }
    }
}
