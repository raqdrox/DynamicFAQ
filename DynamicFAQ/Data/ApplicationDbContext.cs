using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DynamicFAQ.Models;

namespace DynamicFAQ.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DynamicFAQ.Models.Section> Section { get; set; }
        public DbSet<DynamicFAQ.Models.QuestionAnswer> QuestionAnswer { get; set; }
    }
}
