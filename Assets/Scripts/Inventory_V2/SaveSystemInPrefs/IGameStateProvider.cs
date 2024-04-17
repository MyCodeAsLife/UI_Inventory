namespace Inventory
{
    public interface IGameStateProvider
    {
        public void SaveGameState();       // Нужно переделать через асинхрон? void -> async
        public void LoadGameState();       // Нужно переделать через асинхрон? void -> async
    }
}