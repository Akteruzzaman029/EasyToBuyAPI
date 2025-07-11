using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class AspNetRoleClaimsModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string RoleId { get; set; } = string.Empty;

        public string ClaimType { get; set; } = string.Empty;

        public string ClaimValue { get; set; } = string.Empty;
    }
}
