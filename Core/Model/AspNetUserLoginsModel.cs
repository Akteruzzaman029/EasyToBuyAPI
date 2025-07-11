using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class AspNetUserLoginsModel
    {
        [Required]
        [MaxLength(450)]
        public string LoginProvider { get; set; } = string.Empty;

        [Required]
        [MaxLength(450)]
        public string ProviderKey { get; set; } = string.Empty;

        public string ProviderDisplayName { get; set; } = string.Empty;

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = string.Empty;

    }
}
