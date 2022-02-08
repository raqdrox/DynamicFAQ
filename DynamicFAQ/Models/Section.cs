using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicFAQ.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionAnswer> Data { get; set; }

    }
}
