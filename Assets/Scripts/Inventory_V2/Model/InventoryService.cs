using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    // Слой абстрации позволяющий хранить много инвентарей
    // Здесь происходит сохранение при изменении состояния инвентаря
    public class InventoryService  
    {
        private readonly Dictionary<int, InventoryGrid> InventoriesMap = new();
        private readonly IGameStateSaver GameStateSaver;

        public InventoryService(IGameStateSaver gameStateSaver)
        {
            GameStateSaver = gameStateSaver;
        }

        public InventoryGrid RegisterInventory(InventoryGridData inventoryData)     // Зачем вовращать inventory?
        {
            var inventory = new InventoryGrid(inventoryData);
            InventoriesMap[inventory.OwnerId] = inventory;

            return inventory;
        }

        public AddItemsToInventoryGridResult AddItemsToInventory(int ownerId, int itemId, int amount = 1)
        {
            var inventory = InventoriesMap[ownerId];       //+
            var result = inventory.AddItems(itemId, amount);
            GameStateSaver.SaveGameState();

            return result;
        }

        public AddItemsToInventoryGridResult AddItemsToInventory(int ownerId, Vector2Int slotCoords, int itemId, int amount = 1)
        {
            var inventory = InventoriesMap[ownerId];       //+
            var result = inventory.AddItems(slotCoords, itemId, amount);
            GameStateSaver.SaveGameState();

            return result;
        }

        public RemoveItemsFromInventoryGridResult RemoveItems(int ownerId, int itemId, int amount = 1)
        {
            var inventory = InventoriesMap[ownerId];    //+
            var result = inventory.RemoveItems(itemId, amount);
            GameStateSaver.SaveGameState();

            return result;
        }

        public RemoveItemsFromInventoryGridResult RemoveItemsFromSlot(int ownerId, Vector2Int slotCoords, int itemId, int amount = 1)
        {
            var inventory = InventoriesMap[ownerId];       //+
            var result = inventory.RemoveItems(slotCoords, itemId, amount);
            GameStateSaver.SaveGameState();

            return result;
        }

        public IReadOnlyInventoryGrid GetInventory(int ownerId)
        {
            return InventoriesMap[ownerId];
        }
    }
}
