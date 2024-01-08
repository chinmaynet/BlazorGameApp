using BlazorGameApp.Server.Data;
using BlazorGameApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorGameApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly DataContext _context;

        public UnitController(DataContext context)
        {
            _context = context;
        }
        //public IList<Unit> Units => new List<Unit> {
        //    new Unit { Id=1, Title = "Knight", Attack =10, Defence=10, BananaCost =100},
        //    new Unit { Id=2, Title = "Archer", Attack =15, Defence=5, BananaCost =150},
        //    new Unit { Id=3, Title = "Mage", Attack =20, Defence=1, BananaCost =200}
        //};

        //https://localhost:7059/api/unit/GetUnits
        [HttpGet("GetUnits")]
        public async Task<IActionResult>GetUnits()
        {   
            var units =await _context.Units.ToListAsync();
            return Ok(units);
        }


        [HttpPost("AddUnit")]
        public async Task<IActionResult> AddUnit(Unit unit) {
           
            await _context.Units.AddAsync(unit);
            await _context.SaveChangesAsync();
            //return Ok(unit);
            return Ok(await _context.Units.ToListAsync());
        }

        [HttpPut("UpdateUnit/{id}")]
        public async Task<IActionResult> UpdateUnit(int id, Unit unit) {
            Unit dbUnit = await _context.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (dbUnit == null) {
                return NotFound("Unit with given ID not Found");
            }

            dbUnit.Title = unit.Title;
            dbUnit.Attack = unit.Attack;
            dbUnit.Defence = unit.Defence;
            dbUnit.HitPoints = unit.HitPoints;
            dbUnit.BananaCost = unit.BananaCost;

            _context.Units.Update(dbUnit);  //optional line
            await _context.SaveChangesAsync();

            return Ok(dbUnit);
        }

        [HttpDelete("DeleteUnit/{id}")]

        public async Task<IActionResult> DeleteUnit(int id) {
            Unit dbUnit = await _context.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (dbUnit == null)
            {
                return NotFound("Unit with given ID not Found");
            }


            _context.Units.Remove(dbUnit);
            await _context.SaveChangesAsync();

            return Ok(dbUnit);
        }
    }
}
