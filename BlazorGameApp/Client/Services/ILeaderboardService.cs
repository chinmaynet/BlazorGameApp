using BlazorGameApp.Client.Shared;

namespace BlazorGameApp.Client.Services
{
    public interface ILeaderboardService
    {
        IList<UserStatistic> Leaderboard { get; set; }

        Task GetLeaderboard();
    }
}
