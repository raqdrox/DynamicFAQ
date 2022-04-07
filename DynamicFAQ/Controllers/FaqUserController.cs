using System.Threading.Tasks;
using DynamicFAQ.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicFAQ.Controllers
{
    public class FaqUserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FaqUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Section.Include(m => m.Data).ToListAsync());
        }
    }
}