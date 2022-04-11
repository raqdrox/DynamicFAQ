using System.ComponentModel.DataAnnotations;

namespace DynamicFAQ.Models
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
    }
}