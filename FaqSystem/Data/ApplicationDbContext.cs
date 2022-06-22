using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FaqSystem.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FaqSystem.Models.FaqSection> FaqSection { get; set; }

        public DbSet<FaqSystem.Models.FaqArticle> FaqArticle { get; set; }

        public DbSet<FaqSystem.Models.FaqQuestion> FaqQuestion { get; set; }
    }
