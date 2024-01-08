﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGameApp.Shared
{
    public class UserLogin
    {
        [Required(ErrorMessage ="Please Enter Email")]
        public string Email { get; set;}
        [Required]
        public string Password { get; set;} 
    }
}