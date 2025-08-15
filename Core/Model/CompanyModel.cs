using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class CompanyModel : BaseEntity
    {
        public string Code { get; set; } =string.Empty; 
        public string Name { get; set; } = string.Empty;    
        public string ShortName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty; 


    }
}
