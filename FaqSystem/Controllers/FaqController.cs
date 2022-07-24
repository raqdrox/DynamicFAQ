using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaqSystem.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FaqSystem.Controllers
{
    public class FaqController : Controller
    {
        private readonly ApplicationDbContext _context;

        private List<FaqSection> _sectionListFull;
        private List<FaqSection> _sectionListLite;


        public FaqController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Faq/Title
        public async Task<IActionResult> Index(int secId,int qId)
        {
            var secListQuery= _context.FaqSection.Include(sec => sec.QList).Where(x=>x.Id!=0);
            var secList= await secListQuery.ToListAsync();
            FaqQuestion qData;
            if (secList.Count == 0)
                return NotFound();
            secId = secId < secList.Count ? secId : 0;
            qId = qId < secList[secId].QList.Count ? qId : 0;

            if(secList[secId].QList.Count!=0){
                qData =await _context.FaqQuestion.Include(x=>x.Article).Where(x=>x.Id== secList[secId].QList[qId].Id).FirstOrDefaultAsync();
                if (qData == null)
                {
                    return NotFound();
                }
            }
            else
            {
                qData = new FaqQuestion(-1,"",new FaqArticle(-1,""));
            }
            var tuple = new Tuple<IEnumerable<FaqSection>, FaqQuestion>(secList, qData);
                return View(tuple);
            


        }
    }
}
