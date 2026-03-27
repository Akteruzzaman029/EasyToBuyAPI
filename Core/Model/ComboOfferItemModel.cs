using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class ComboOfferItemModel : BaseEntity
    {
        public int ComboOfferId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal ComboPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public int? SequenceNo { get; set; }
    }
}
