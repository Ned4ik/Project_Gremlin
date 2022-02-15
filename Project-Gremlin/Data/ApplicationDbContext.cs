using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_Gremlin.Models;
using Project_Gremlin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Gremlin.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<MiniHistory> MiniHistory { get; set; }
        public DbSet<Character> Character { get; set; }
        public DbSet<Lore> Lore { get; set; }
        
    }
}
