using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaqSystem.Models
{
    public class FaqSection
    {
        public int Id { get; set; }
        [Required]public string SectionTitle { get; set; }
        public List<FaqQuestion> QList { get; set; }

        public FaqSection(int id,string title, List<FaqQuestion> qlist)
        {
            Id = id;
            SectionTitle = title;
            if (qlist == null)
            {
                QList = new List<FaqQuestion>();
            }
            else
            {
                QList = qlist;
            }
        }

        public FaqSection(FaqSection faqSection)
        {
            Id = faqSection.Id;
            SectionTitle = faqSection.SectionTitle;
            QList = faqSection.QList;
        }

        public FaqSection()
        {

        }

        public FaqSection ShallowCopy()
        {
            return new FaqSection(Id, SectionTitle,null);
        }
    }
}
