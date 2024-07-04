using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Password { get; set; }
    }
}
