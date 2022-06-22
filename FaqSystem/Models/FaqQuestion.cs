using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaqSystem.Models
{
    public class FaqQuestion
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; }
        public FaqArticle Article { get; set; }

        public FaqQuestion(int id, string title, FaqArticle article)
        {
            Id = id;
            Title = title;
            if (article == null)
            {
                Article = new FaqArticle();
            }
            else
            {
                Article = article;
            }
        }

        public FaqQuestion(FaqQuestion faqQuestion)
        {
            Id = faqQuestion.Id;
            Title = faqQuestion.Title;
            Article = faqQuestion.Article;
        }
        public FaqQuestion()
        {
            
        }
        public FaqQuestion ShallowCopy()
        {
            return new FaqQuestion(Id, Title,null);
        }
    }
}
