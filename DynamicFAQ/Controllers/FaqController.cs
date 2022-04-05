using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DynamicFAQ.Data;
using DynamicFAQ.Models;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace DynamicFAQ.Controllers
{
    public class FaqController : Controller
    {

        private readonly ApplicationDbContext _db;

        public FaqController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Faq
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _db.Section.Include(m => m.Data).ToListAsync());
        }




        //----------------------------------------------------------------------------------------------------------



        // GET: Faq/CreateSection
        [Authorize]
        public IActionResult CreateSection()
        {
            return View();
        }

        // POST: Faq/CreateSection
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSection([Bind("Id,Name")] Section section)
        {
            if (ModelState.IsValid)
            {
                _db.Add(section);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(section);
        }





        //----------------------------------------------------------------------------------------------------------

        // GET: Faq/Edit/{SectionId}
        [Authorize]
        public async Task<IActionResult> EditSection(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _db.Section.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            return View(section);
        }

        // POST: Faq/Edit/{SectionId}
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSection(int id, [Bind("Id,Name")] Section section)
        {
            if (id != section.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(section);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionExists(section.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(section);
        }



        //----------------------------------------------------------------------------------------------------------



        // GET: Faq/Edit/{FaqId}
        [Authorize]
        public async Task<IActionResult> EditFaq(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qa = await _db.QuestionAnswer.FindAsync(id);
            if (qa == null)
            {
                return NotFound();
            }
            return View(qa);
        }

        // POST: Faq/Edit/{FaqId}
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFaq(int id, [Bind("Id,Question,Answer")] QuestionAnswer qa)
        {
            if (id != qa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(qa);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaqExists(qa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(qa);
        }





        //----------------------------------------------------------------------------------------------------------



        // GET: Faq/Delete/{SectionId}
        [Authorize]
        public async Task<IActionResult> DeleteSection(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _db.Section.Include(m => m.Data)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // POST: Faq/Delete/{SectionId}
        [Authorize]
        [HttpPost, ActionName("DeleteSection")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSectionConfirmed(int id)
        {
            var section = await _db.Section.Include(m => m.Data)
                .FirstOrDefaultAsync(m => m.Id == id);
            List<int> itemIdList=new List<int>();
            foreach (var item in section.Data)
            {
                itemIdList.Add(item.Id);
            }

            foreach (var item in itemIdList)
            {
                var qa = await _db.QuestionAnswer.FirstOrDefaultAsync(m => m.Id == item);
                _db.QuestionAnswer.Remove(qa);
                await _db.SaveChangesAsync();
            }
            
            _db.Section.Remove(section);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        //----------------------------------------------------------------------------------------------------------



        // GET: Faq/DeleteFaq/{FaqId}
        [Authorize]
        public async Task<IActionResult> DeleteFaq(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qa = await _db.QuestionAnswer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qa == null)
            {
                return NotFound();
            }

            return View(qa);
        }

        // POST: Faq/DeleteFaq/{FaqId}
        [Authorize]
        [HttpPost, ActionName("DeleteFaq")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFaqConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var qa = await _db.QuestionAnswer.FirstOrDefaultAsync(m => m.Id == id);
            if (qa == null)
            {
                return NotFound();
            }
            _db.QuestionAnswer.Remove(qa);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        //----------------------------------------------------------------------------------------------------------


        // GET: Faq/CreateFaq/{SectionId}
        [Authorize]
        [HttpGet]
        public IActionResult CreateFaq(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            return View();
        }

        // POST: Faq/CreateFaq/{SectionId}
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaq(int? id,[Bind("Question,Answer")] QuestionAnswer questionAnswer)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var addedFaq=_db.QuestionAnswer.Add(questionAnswer);
                addedFaq.Property("SectionId").CurrentValue= id;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        //----------------------------------------------------------------------------------------------------------


        // GET: Faq/ReorderFaq/{SectionId}
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ReorderFaq(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var section = await _db.Section.Include(m => m.Data)
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(section);
        }

        // POST: Faq/ReorderFaq/{SectionId}
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReorderFaq(int? id, [Bind("Question,Answer")] QuestionAnswer questionAnswer)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var addedFaq = _db.QuestionAnswer.Add(questionAnswer);
                addedFaq.Property("SectionId").CurrentValue = id;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        //----------------------------------------------------------------------------------------------------------


        private bool SectionExists(int id)
        {
            return _db.Section.Any(e => e.Id == id);
        }
        private bool FaqExists(int id)
        {
            return _db.QuestionAnswer.Any(e => e.Id == id);
        }
    }
}
