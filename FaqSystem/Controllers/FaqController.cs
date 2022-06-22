using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaqSystem.Models;

namespace FaqSystem.Controllers
{
    public class FaqController : Controller
    {
        private readonly ApplicationDbContext _context;

        private List<FaqSection> _sectionListFull;
        private List<FaqSection> _sectionListLite;

        public async void RefreshSectionList()
        {
            _sectionListFull = await _context.FaqSection.Include(sec=>sec.QList).ThenInclude(ques=>ques.Article).ToListAsync();
            _sectionListFull = _sectionListFull.OrderBy(x=>x.Id).ToList();
            _sectionListFull.Remove(_sectionListFull.Single(s => s.Id == 0));
            _sectionListLite = new List<FaqSection>();
            foreach (var faqSection in _sectionListFull)
            {
                faqSection.QList = faqSection.QList.OrderBy(x => x.Id).ToList();
                var copy = faqSection.ShallowCopy();
                foreach (var faqQuestion in faqSection.QList)
                {
                    copy.QList.Add(faqQuestion.ShallowCopy());
                }
                _sectionListLite.Add(copy);
                
            }

            
        }

        public FaqController(ApplicationDbContext context)
        {
            _context = context;
            RefreshSectionList();
        }

        // GET: Faq/Title
        public async Task<IActionResult> Index(int secId,int qId)
        {

            secId = secId < _sectionListLite.Count ? secId : 0;
            qId = qId < _sectionListLite[secId].QList.Count ? qId : 0;


            var qData= _sectionListFull[secId].QList[qId];
            var tuple = new Tuple<IEnumerable<FaqSection>, FaqQuestion>(_sectionListLite, qData);
            return View(tuple);

        }
        
    }
}
