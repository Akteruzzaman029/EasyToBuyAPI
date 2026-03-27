using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class ComboOfferModel : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TypeTag { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int FileId { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal ComboPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFreeItem { get; set; }
        public int? SequenceNo { get; set; }
    }
}
