using Blazored.Toast.Services;
using BlazorGameApp.Shared;
using System.Net.Http.Json;

namespace BlazorGameApp.Client.Services
{
    public class UnitService : IUnitService
    {
        private readonly IToastService _toastService;
        private readonly HttpClient _http;
        private readonly IBananaService _bananaService;

        public UnitService(IToastService toastService, HttpClient http, IBananaService bananaService)
        {
            _toastService = toastService;
            _http = http;
            _bananaService = bananaService;
        }
        //public IList<Unit> Units => new List<Unit> {
        //    new Unit { Id=1, Title = "Knight", Attack =10, Defence=10, BananaCost =100},
        //    new Unit { Id=2, Title = "Archer", Attack =15, Defence=5, BananaCost =150},
        //    new Unit { Id=3, Title = "Mage", Attack =20, Defence=1, BananaCost =200}
        //};

        public IList<Unit> Units{ get ; set ; } = new List<Unit>();

        public IList<UserUnit> MyUnits { get ; set ; } = new List<UserUnit>();
        
        public async Task AddUnit(int unitId)
        {
            var unit = Units.First(unit => unit.Id == unitId);
            //MyUnits.Add(new UserUnit { UnitId = unit.Id, HitPoints = unit.HitPoints });
            var result = await _http.PostAsJsonAsync<int>("api/userunit", unitId);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _toastService.ShowError(await result.Content.ReadAsStringAsync());
            }
            else {
                await _bananaService.GetBananas();
                _toastService.ShowSuccess($"Your {unit.Title} Added Successfully!");
            }
           
            //api call post MyUnits
            //_toastService.ShowSuccess("Unit Added Successfully!");
      
            //Console.WriteLine($"{unit.Title} was build");
            //Console.WriteLine($"Your Army Size: {MyUnits.Count}");
        }

        public async Task LoadUnitsAsync()
        {
            if (Units.Count == 0) {
                Units = await _http.GetFromJsonAsync<IList<Unit>>("api/unit/GetUnits");            
            }
        }

        public async Task LoadUserUnitsAsync()
        {
            MyUnits = await _http.GetFromJsonAsync<IList<UserUnit>>("api/userunit");
        }

        public async Task ReviveArmy()
        {
            var result = await _http.PostAsJsonAsync<string>("api/userunit/revivearmy",null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _toastService.ShowSuccess(await result.Content.ReadAsStringAsync());
            }
            else {             
                _toastService.ShowError(await result.Content.ReadAsStringAsync());        
            }

            await LoadUserUnitsAsync();
            await _bananaService.GetBananas();
        }
    }
}
