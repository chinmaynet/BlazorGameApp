﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGameApp.Shared
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }    

        public byte[] PasswordSalt { get; set; }

        public int Bananas { get; set; }

        public DateTime DateOfBirth { get; set; }
        public bool Isconfirmed { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public List<UserUnit> Units { get; set; }  //one to many relationship, one usesr can have multiple user unit (an army)

        public int Battles { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}