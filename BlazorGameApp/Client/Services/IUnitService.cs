using BlazorGameApp.Shared;

namespace BlazorGameApp.Client.Services
{
 
    public interface IUnitService
    {   
        IList<Unit> Units { get; set; } //kinds of unit  available to build
        IList<UserUnit> MyUnits { get; set; } // list of all units current user ownes

        Task AddUnit(int unitId); //add unit to that list
        Task LoadUnitsAsync();

        Task LoadUserUnitsAsync();

        Task ReviveArmy();
    }
}
