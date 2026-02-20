using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.WebSite;


public class WebsiteSection : BaseEntity
{
    public string? Name { get; set; }
    public string? HeaderName { get; set; }
    public int? SequenceNo { get; set; }
    public int? FileId { get; set; }
}
