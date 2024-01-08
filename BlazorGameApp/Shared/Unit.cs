﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGameApp.Shared
{
    public class Unit
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int Attack { get; set; }

        public int Defence { get; set; }

        public int HitPoints { get; set; } = 100;
        public int BananaCost { get; set; }
    }
}