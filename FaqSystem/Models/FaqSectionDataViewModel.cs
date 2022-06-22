using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaqSystem.Models
{
    public class FaqSectionDataViewModel
    {
        [Required] public int Id { get; set; }
        [Required] public string SectionTitle { get; set; }
        [Required] public int QlistCount { get; set; }
    }
}
