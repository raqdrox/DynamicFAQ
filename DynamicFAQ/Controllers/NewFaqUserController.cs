using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DynamicFAQ.Data;
using DynamicFAQ.Models;

namespace DynamicFAQ.Controllers
{
    public class NewFaqUserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewFaqUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Section.Include(m => m.Data).ToListAsync());
        }


    }
}
