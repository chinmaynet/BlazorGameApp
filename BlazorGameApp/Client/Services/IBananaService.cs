namespace BlazorGameApp.Client.Services
{
    public interface IBananaService
    {
        event Action onchange;
        int Bananas { get; set; }

        void EatBananas(int amount);

        Task AddBananas(int amount);

        Task GetBananas();
    }
}
