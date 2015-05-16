using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountActivation.Models
{
    public class CreateUserModel
    {
        [Required]
        [Display(Name="Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}