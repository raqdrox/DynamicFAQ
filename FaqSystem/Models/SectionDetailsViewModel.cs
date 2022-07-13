using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaqSystem.Models
{
    public enum SectionDetailsMode
    {
        Details,
        EditSection,
        DeleteSection

    }
    public class SectionDetailsViewModel
    {
        public FaqSection Section;
        public SectionDetailsMode Mode;
        public int ModeId;
    }
}
