using System.Collections.Generic;

namespace DynamicFAQ.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionAnswer> Data { get; set; }
    }
}