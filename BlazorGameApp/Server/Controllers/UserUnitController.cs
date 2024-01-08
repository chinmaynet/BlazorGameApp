using BlazorGameApp.Server.Data;
using BlazorGameApp.Server.Services;
using BlazorGameApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlazorGameApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserUnitController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUtilityService _utilityService;

        public UserUnitController(DataContext context, IUtilityService utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }


        [HttpPost("revivearmy")]
        public async Task<IActionResult> ReviveArmy() {
            var user = await _utilityService.GetUser();
            var userUnits = await _context.UserUnits
                .Where(unit => unit.UserId == user.Id)
                .Include(unit => unit.unit)
                .ToListAsync();

            int bananaCost = 1000;
            if (userUnits.IsNullOrEmpty()) {
               return  BadRequest("Error, Add Army First");
            }
            if (user.Bananas < bananaCost) {
                return BadRequest("Not Enough Bananas! Need 1000 to Revive Army!");
            }

            bool isArmyAlreadyAlive = true;
            foreach (var userUnit in userUnits) {
                if (userUnit.HitPoints <= 0) {
                    isArmyAlreadyAlive = false;
                    userUnit.HitPoints = new Random().Next(0, userUnit.unit.HitPoints);
                }
            }

            if (isArmyAlreadyAlive)
            {
                return Ok("Your Army is Already Alive");
            }
            user.Bananas -= bananaCost;
            await _context.SaveChangesAsync();

            return Ok("Army Revived!");            
        }

      
        [HttpPost]
        public async Task<IActionResult> BuildUnit([FromBody] int unitId)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(u => u.Id == unitId);
            var user = await _utilityService.GetUser();

            if (user.Bananas < unit.BananaCost)
            {
                return BadRequest("Not Enough Bananas");
            }

            user.Bananas -= unit.BananaCost;

            var newUserUnit = new UserUnit
            {
                UserId = user.Id,
                UnitId = unit.Id,
                HitPoints = unit.HitPoints,
            };
            _context.UserUnits.Add(newUserUnit);
            await _context.SaveChangesAsync();

            return Ok(newUserUnit);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserUnits()
        {
            var user = await _utilityService.GetUser();
            var userUnits = await _context.UserUnits.Where(unit => unit.UserId== user.Id).ToListAsync();

            var response = userUnits.Select(
                unit => new UserUnitResponse { 
                    UnitId= unit.UnitId,
                    HitPoints= unit.HitPoints
                });

            return Ok(response);
        }
    }
}
