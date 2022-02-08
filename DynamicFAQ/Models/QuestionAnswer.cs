using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicFAQ.Models
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

    }
}
