using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FaqSystem.Models
{
    public class FaqArticle
    {
        public int Id { get; set; }
        [Column(TypeName = "CLOB")] public string Contents { get; set; }

        public FaqArticle(int id, string contents)
        {
            Id = id;
            Contents = contents;
        }
        public FaqArticle(FaqArticle faqArticle)
        {
            Id = faqArticle.Id;
            Contents = faqArticle.Contents;
        }
        public FaqArticle()
        {

        }
    }
}
