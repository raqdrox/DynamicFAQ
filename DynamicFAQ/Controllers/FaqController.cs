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
using Microsoft.AspNetCore.Authorization;

namespace DynamicFAQ.Controllers
{
    public class FaqController : Controller
    {

        private readonly ApplicationDbContext _db;
        private static int _currentSectionId;

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

        // GET: Faq/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _db.Section.Include(m =>m.Data)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (section == null)
            {
                return NotFound();
            }

            _currentSectionId = section.Id;
            return View(section);
        }

        // GET: Faq/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faq/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Section section)
        {
            if (ModelState.IsValid)
            {
                _db.Add(section);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(section);
        }

        // GET: Faq/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Faq/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Section section)
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

        // GET: Faq/Edit/5
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

        // POST: Faq/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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


        // GET: Faq/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Faq/Delete/0
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var section = await _db.Section.FindAsync(id);
            _db.Section.Remove(section);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Faq/DeleteFaq/5
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

        // POST: Faq/DeleteFaq/5
        [Authorize]
        [HttpPost, ActionName("DeleteFaq")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFaqConfirmed(int id)
        {
            var qa = await _db.QuestionAnswer.FindAsync(id);
            _db.QuestionAnswer.Remove(qa);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Faq/Section/CreateFaq
        [Authorize]
        public IActionResult CreateFaq()
        {
            return View();
        }

        // POST: Faq/Section/CreateFaq
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaq([Bind("Id,Question,Answer")] QuestionAnswer questionAnswer)
        {
            if (ModelState.IsValid)
            {

                var addedFaq=_db.QuestionAnswer.Add(questionAnswer);
                addedFaq.Property("SectionId").CurrentValue=_currentSectionId;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }




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
