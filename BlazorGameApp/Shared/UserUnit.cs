using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGameApp.Shared
{
    public class UserUnit
    {   
        public int Id { get; set; } 
        public int UserId { get; set; } //fk to user

        public Unit unit { get; set; }         //unit obj
        public int UnitId { get; set; } //fk to unit
        
        public int HitPoints { get; set; }
    }
}
