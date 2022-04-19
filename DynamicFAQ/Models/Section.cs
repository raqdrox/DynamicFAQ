﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DynamicFAQ.Models
{
    public class Section
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        public List<QuestionAnswer> Data { get; set; }
    }
}