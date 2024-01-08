using System.Net.Http.Json;

namespace BlazorGameApp.Client.Services
{
    public class BananaService : IBananaService
    {
        private readonly HttpClient _http;

        public BananaService(HttpClient http)
        {
            _http = http;
        }
        public event Action onchange;
        public int Bananas { get; set; } = 1000;

        public void EatBananas(int amount)
        {   
            Bananas -= amount;

            bananasChanged();
        }        

        public async Task AddBananas(int amount)
        {
            //Bananas += amount;

            var result = await _http.PutAsJsonAsync<int>("api/user/addbananas",amount);
            Bananas = await result.Content.ReadFromJsonAsync<int>();           
            bananasChanged();
        }

        public async Task GetBananas()
        {
            Bananas = await _http.GetFromJsonAsync<int>("api/user/getbananas");
            bananasChanged();
        }

        void bananasChanged() => onchange.Invoke();
    }
}
