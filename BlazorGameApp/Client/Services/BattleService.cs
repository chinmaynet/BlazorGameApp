using BlazorGameApp.Shared;
using System.Net.Http.Json;

namespace BlazorGameApp.Client.Services
{
    public class BattleService : IBattleService
    {
        private readonly HttpClient _http;

        public BattleService(HttpClient http)
        {
            _http = http;
        }

        public BattleResult LastBattle { get; set; } = new BattleResult();
        public IList<BattleHistoryEntry> History { get ; set; } = new List<BattleHistoryEntry>();

        public async Task GetHistory()
        {
            History = await _http.GetFromJsonAsync<BattleHistoryEntry[]>("api/user/history");
        }

        public async Task<BattleResult> StartBattle(int opponentId)
        {
            var result =await  _http.PostAsJsonAsync<int>("/api/battle",opponentId);
            LastBattle =  await result.Content.ReadFromJsonAsync<BattleResult>();

            return LastBattle;
        }
    }
}
