using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory
{
    public class GameStatePlayerPrefsProvider : IGameStateProvider, IGameStateSaver
    {
        private const string Key = "GAME STATE";

        public GameStateData GameState { get; private set; }

        public void SaveGameState()
        {
            var json = JsonUtility.ToJson(GameState);
            PlayerPrefs.SetString(Key, json);
        }

        public void LoadGameState()         // Должен быть асинхранным
        {
            if (PlayerPrefs.HasKey(Key)) // Если есть ключ, значит есть что загружать?
            {
                var json = PlayerPrefs.GetString(Key);
                json = json.NullIfEmpty();      // Если вдруг в префсах какимто образом образовался ключ а к нему ничего

                if (json != null)
                {
                    GameState = JsonUtility.FromJson<GameStateData>(json);
                    return;
                }
            }

            GameState = InitFromSettings();
            SaveGameState();
        }

        private GameStateData InitFromSettings()    // Здесь происходит инициализация конфига
        {
            // С имитируем инициализацию конфига
            var gameState = new GameStateData
            {
                Inventories = new List<InventoryGridData>
            {
                CreateTestInventory(007),
                CreateTestInventory(1477),
            }
            };

            return gameState;
        }

        private InventoryGridData CreateTestInventory(int ownerId)      // Тестовое создание инвентаря
        {
            var size = new Vector2Int(3, 4);                            // Размер инвентаря
            var createdInventorySlots = new List<InventorySlotData>();
            int length = size.x * size.y;   //+

            for (int i = 0; i < length; i++)
                createdInventorySlots.Add(new InventorySlotData());     // Создание пустых слотов

            var createdInventoryData = new InventoryGridData            // Создание Инвентаря
            {
                OwnerId = ownerId,                                      // ID Инвентаря
                Size = size,                                            // Размер инвентаря
                Slots = createdInventorySlots,                          // Созданные выше пустые слоты для инвентаря
            };

            return createdInventoryData;                                // Возвращаем созданный инвентарь
        }
    }
}