using BlazorGameApp.Client.Shared;
using BlazorGameApp.Server.Data;
using BlazorGameApp.Server.Services;
using BlazorGameApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorGameApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUtilityService _utilityService;

        public UserController(DataContext context, IUtilityService utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }
        //removed this in utilityService
        //private int GetUserId()=> int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //private async Task<User> GetUser() => await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());


        [HttpGet("getbananas")]
        public async Task<IActionResult> GetBananas()
        {


            //var user = await GetUser();
            var user = await _utilityService.GetUser();
            if (user == null) { return BadRequest("Error Occured"); }
            else
            {
                return Ok(user.Bananas);
            }

        }

        [HttpPut("addbananas")]

        public async Task<IActionResult> AddBananas([FromBody] int bananas)
        {
            var user = await _utilityService.GetUser();
            user.Bananas += bananas;

            await _context.SaveChangesAsync();
            return Ok(user.Bananas);

        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var users = await _context.Users.Where(user => user.Isconfirmed && !user.IsDeleted).ToListAsync();
            users = users.OrderByDescending(u => u.Victories)
                            .ThenBy(u => u.Defeats)
                            .ThenBy(u => u.DateCreated)
                            .ToList();
            int rank = 1;

            var response = users.Select(user => new UserStatistic
            {
                Rank = rank++,
                UserId = user.Id,
                UserName = user.UserName,
                Battles = user.Battles,
                Victories = user.Victories,
                Defeats = user.Defeats
            });

            return Ok(response);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var user = await _utilityService.GetUser();
            var battles = await _context.Battles
                .Where(battle => battle.AttackerId == user.Id || battle.OpponentId == user.Id)
                .Include(battle => battle.Attacker)
                .Include(battle => battle.Opponent)
                .Include(battle => battle.Winner)
                .ToListAsync();
            var history = battles.Select(battle => new BattleHistoryEntry {
                BattleId= battle.Id,
                BattleDate   = battle.BattleDate,
                AttackerId = battle.AttackerId,
                OpponentId = battle.OpponentId,
                YouWon = battle.WinnerId==user.Id,
                AttackerName = battle.Attacker.UserName,
                OpponentName = battle.Opponent.UserName,
                RoundsFought = battle.RoundsFought,
                WinnerDamageDealt = battle.WinnerDamage
            });

            return Ok(history.OrderByDescending(h=>h.BattleDate));
        }
    }
}
