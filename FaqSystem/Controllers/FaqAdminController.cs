using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FaqSystem.Models;
using Ganss.XSS;

namespace FaqSystem.Controllers
{
    public class FaqAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<FaqSection> _sectionList;
        private HtmlSanitizer _sanitizer;
        public FaqAdminController(ApplicationDbContext context)
        {
            _context = context;
            InitHtmlSanitizer();
            RefreshSectionList();
        }

        public async void RefreshSectionList()
        {
            _sectionList = await _context.FaqSection
                .Include(sec => sec.QList)
                .ThenInclude(ques => ques.Article)
                .ToListAsync();
            _sectionList = _sectionList.OrderBy(x => x.Id).ToList();
            foreach (var faqSection in _sectionList)
            {
                faqSection.QList = faqSection.QList.OrderBy(x => x.Id).ToList();
            }
        }

        // GET: FaqAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.FaqSection.ToListAsync());
        }

        private void InitHtmlSanitizer()
        {
            _sanitizer = new HtmlSanitizer(allowedTags: HtmlSanitizer.DefaultAllowedTags, allowedAttributes: HtmlSanitizer.DefaultAllowedAttributes);
            _sanitizer.AllowedAttributes.Add("src");
            _sanitizer.AllowedSchemes.Add("data");

        }








        // GET: FaqAdmin/Details/5
        public async Task<IActionResult> SectionDetails(int? id,string mode="details")
        {
            if (id == null)
            {
                return NotFound();
            }

            mode = mode.ToLower();

            var faqSection = await _context.FaqSection.Include(q=>q.QList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faqSection == null)
            {
                return NotFound();
            }

            ViewData["Mode"] = mode;
            return View(faqSection);
        }


        // GET: FaqAdmin/Details/5
        public async Task<IActionResult> QuestionDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            FaqQuestionViewModel faqQuestionViewModel = new FaqQuestionViewModel();

            Tuple<FaqQuestion, int, int> questionSection = GetQuestionSectionByQId((int)id);
            if (questionSection == null)
            {
                return NotFound();
            }
            var faqSection = await _context.FaqSection.ToListAsync();
            if (faqSection == null)
            {
                return NotFound();
            }
            faqQuestionViewModel.SectionId = questionSection.Item2;
            faqQuestionViewModel.QuestionId = id.Value;
            faqQuestionViewModel.QuestionTitle = questionSection.Item1.Title;
            faqQuestionViewModel.ArticleContents = questionSection.Item1.Article.Contents;
            faqQuestionViewModel.Sections = faqSection;
            
            
            return View(faqQuestionViewModel);
        }















        // GET: FaqAdmin/CreateSection
        public IActionResult CreateSection()
        {
            return View();
        }

        // POST: FaqAdmin/CreateSection
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSection([Bind("Id,SectionTitle")] FaqSection faqSection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faqSection);
                await _context.SaveChangesAsync();
                RefreshSectionList();
                return RedirectToAction(nameof(Index));
            }
            return View(faqSection);
        }













        // GET: FaqAdmin/CreateQuestion
        public async Task<IActionResult> CreateQuestion()
        {
            var faqSection = await _context.FaqSection.ToListAsync();
            ViewBag.sectionList = faqSection;
            return View();
        }

        // POST: FaqAdmin/CreateQuestion
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion([Bind("SectionId", "QuestionTitle", "ArticleContents")] FaqQuestionViewModel questionViewModel)
        {
            WriteDataToDebugFile(questionViewModel.ArticleContents);
            
            var sanitizedArticle = _sanitizer.Sanitize(questionViewModel.ArticleContents);
            FaqArticle article = new FaqArticle(0, sanitizedArticle);
            FaqQuestion ques = new FaqQuestion(0, questionViewModel.QuestionTitle, article);
            var sec = _sectionList.FirstOrDefault(m => m.Id == questionViewModel.SectionId);
            if (sec == null)
            {
                return NotFound();
            }
            sec.QList.Add(ques);
            if (!ModelState.IsValid) return View(questionViewModel);
            _context.Update(sec);
            await _context.SaveChangesAsync();
            RefreshSectionList();
            return RedirectToAction(nameof(Index));
        }

        public void WriteDataToDebugFile(string data)
        {
            string path = @"DebugFile.txt";
            System.IO.File.WriteAllText(path, data);
        }
















        // GET: FaqAdmin/Edit/5
        public async Task<IActionResult> EditSection(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id.Value == 0)
            {
                return NotFound();
            }
            var faqSection = _sectionList.FirstOrDefault(s=>s.Id== id.Value);
            if (faqSection == null)
            {
                return NotFound();
            }
            return View(faqSection);
        }

        // POST: FaqAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSection([Bind("Id,SectionTitle")] FaqSection faqSection)
        {
            
            var sec = _sectionList.FirstOrDefault(s => s.Id == faqSection.Id);
            if (sec == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(faqSection);
            try
            {
                sec.SectionTitle = faqSection.SectionTitle;
                _context.Update(sec);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaqSectionExists(faqSection.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            RefreshSectionList();
            return RedirectToAction(nameof(Index));
        }


















        // GET: FaqAdmin/Edit/5
        public async Task<IActionResult> EditQuestion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            FaqQuestionViewModel faqQuestionViewModel = new FaqQuestionViewModel();

            Tuple<FaqQuestion, int,int> questionSection =GetQuestionSectionByQId((int)id);
            if (questionSection == null)
            {
                return NotFound();
            }

            faqQuestionViewModel.SectionId = questionSection.Item2;
            faqQuestionViewModel.QuestionTitle = questionSection.Item1.Title;
            faqQuestionViewModel.ArticleContents = questionSection.Item1.Article.Contents;
            var faqSection = await _context.FaqSection.ToListAsync();
            ViewBag.sectionList = faqSection;
            return View(faqQuestionViewModel);
        }



        // POST: FaqAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("EditQuestion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestionPost([Bind("SectionId", "QuestionId", "QuestionTitle", "ArticleContents")] FaqQuestionViewModel questionViewModel)
        {
            WriteDataToDebugFile(questionViewModel.ArticleContents);
            

            if (!ModelState.IsValid) return View(questionViewModel);
            Tuple<FaqQuestion, int,int> questionSection = GetQuestionSectionByQId(questionViewModel.QuestionId);
            if (questionSection == null)
            {
                return NotFound();
            }
            try
            {
                var sec = _sectionList.FirstOrDefault(y => y.Id == questionSection.Item2);
                var ques = questionSection.Item1;
                if (sec == null)
                {
                    return NotFound();
                }
                var qpos = questionSection.Item3;
                sec.QList[qpos].Title = questionViewModel.QuestionTitle;
                var sanitizedArticle = _sanitizer.Sanitize(questionViewModel.ArticleContents);
                sec.QList[qpos].Article.Contents = sanitizedArticle;
                if (questionSection.Item2 != questionViewModel.SectionId)
                {
                    var newSec =_sectionList.FirstOrDefault(y => y.Id == questionViewModel.SectionId);
                    if (newSec == null)
                    {
                        return NotFound();
                    }
                    newSec.QList.Add(ques);
                    sec.QList.RemoveAt(qpos);
                    _context.Update(newSec);
                }
                _context.Update(sec);
                await _context.SaveChangesAsync();
                RefreshSectionList();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }















        // GET: FaqAdmin/Delete/5
        public async Task<IActionResult> DeleteSection(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            var faqSection = await _context.FaqSection
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faqSection == null)
            {
                return NotFound();
            }

            return View(faqSection);
        }

        // POST: FaqAdmin/Delete/5
        [HttpPost, ActionName("DeleteSection")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSectionConfirmed(int id)
        {
            var faqSection = _sectionList.FirstOrDefault(m=>m.Id== id);
            var defaultSection = _sectionList.FirstOrDefault(m => m.Id == 0);
            if(defaultSection==null)
            {
                return NotFound();
            }
            if (faqSection == null)
            {
                return NotFound();
            }

            foreach (var faqQuestion in faqSection.QList)
            {
                defaultSection.QList.Add(faqQuestion);
            }
            faqSection.QList.Clear();
            _context.Update(defaultSection);
            _context.Remove(faqSection);
            await _context.SaveChangesAsync();
            RefreshSectionList();
            return RedirectToAction(nameof(Index));
        }










        // GET: FaqAdmin/Delete/5
        public async Task<IActionResult> DeleteQuestion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FaqQuestionViewModel faqQuestionViewModel = new FaqQuestionViewModel();

            Tuple<FaqQuestion, int, int> questionSection = GetQuestionSectionByQId(id.Value);
            if (questionSection == null)
            {
                return NotFound();
            }

            faqQuestionViewModel.SectionId = questionSection.Item2;
            faqQuestionViewModel.QuestionTitle = questionSection.Item1.Title;
            faqQuestionViewModel.ArticleContents = questionSection.Item1.Article.Contents;
            ViewBag.SectionTitle = _sectionList.First(y => y.Id == questionSection.Item2).SectionTitle;
            return View(faqQuestionViewModel);
        }

        // POST: FaqAdmin/Delete/5
        [HttpPost, ActionName("DeleteQuestion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestionConfirmed(int id)
        {
            Tuple<FaqQuestion, int, int> questionSection = GetQuestionSectionByQId((int)id);
            if (questionSection == null)
            {
                return NotFound();
            }
            var sec = _sectionList.FirstOrDefault(y => y.Id == questionSection.Item2);
            var qpos = questionSection.Item3;
            if (sec == null)
            {
                return NotFound();
            }
            sec.QList.RemoveAt(qpos);
            _context.Update(sec);
            await _context.SaveChangesAsync();
            RefreshSectionList();
            return RedirectToAction(nameof(Index));
        }
















        private bool FaqSectionExists(int id)
        {
            return _context.FaqSection.Any(e => e.Id == id);
        }

        //Returns Tuple(FaqQuestion,SectionId,QList Pos)
        //Returns null if not found
        Tuple<FaqQuestion, int,int> GetQuestionSectionByQId(int id)
        {
            foreach (var section in _sectionList)
            {
                foreach (var faqQuestion in section.QList)
                {
                    if (faqQuestion.Id == id)
                    {
                        return new Tuple<FaqQuestion, int,int>(faqQuestion, section.Id,section.QList.IndexOf(faqQuestion));
                    }
                }
            }

            return null;
        }
    }
}
