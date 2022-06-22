using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaqSystem.Models
{
    public class FaqQuestionViewModel
    {
        [Required] public int SectionId { get; set; }
        [Required] public string QuestionTitle { get; set; }
        [Required] public string ArticleContents { get; set; }
    }
}
