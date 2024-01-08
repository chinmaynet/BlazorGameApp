using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGameApp.Shared
{
    public class UserRegister
    {
        [Required, StringLength(16,ErrorMessage ="User Name Is Too Long(Max 16 characters)")]
        public string UserName { get; set; }

        [Required,EmailAddress]
        public string Email { get; set; }
        public string Bio { get; set; }
        [Required, StringLength(100,MinimumLength =6)]
        public string Password { get; set; }
   
        [Required, StringLength(100, MinimumLength = 6), Compare("Password", ErrorMessage = "Passwords Do Not Match)")]
        public string ConfirmPassword { get; set; }


        public int StartUnitId { get; set; } = 1;


        [Required,Range(0,1000, ErrorMessage = "Choose Number between 0 and 1000)")]
        public int Bananas { get; set; } = 100;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        [Required, Range(typeof(bool),"true","true",ErrorMessage = "Only confirmed usesrs can play)")]
        public bool IsConfirmed { get; set; }
    }
}
