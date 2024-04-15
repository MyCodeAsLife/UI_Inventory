using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryService       // Слой абстрации позволяющий хранить много инвентарей
    {
        private readonly Dictionary<int, InventoryGrid> InventoriesMap = new();

        public InventoryGrid RegisterInventory(InventoryGridData inventoryData)     // Зачем вовращать inventory?
        {
            var inventory = new InventoryGrid(inventoryData);
            InventoriesMap[inventory.OwnerId] = inventory;

            return inventory;
        }

        public AddItemsToInventoryGridResult AddItemsToInventory(int ownerId, int itemId, int amount = 1)
        {
            var inventory = InventoriesMap[ownerId];       //+
            return inventory.AddItems(itemId, amount);
        }

        public AddItemsToInventoryGridResult AddItemsToInventory(int ownerId, Vector2Int slotCoords, int itemId, int amount = 1)
        {
            var inventory = InventoriesMap[ownerId];       //+
            return inventory.AddItems(slotCoords, itemId, amount);
        }

        public RemoveItemsFromInventoryGridResult RemoveItems(int ownerId, int itemId, int amount = 1)
        {
            var inventory = InventoriesMap[ownerId];    //+
            return inventory.RemoveItems(itemId, amount);
        }

        public RemoveItemsFromInventoryGridResult RemoveItemsFromSlot(int ownerId, Vector2Int slotCoords, int itemId, int amount = 1)
        {
            var inventory = InventoriesMap[ownerId];       //+
            return inventory.RemoveItems(slotCoords, itemId, amount);
        }

        public IReadOnlyInventoryGrid GetInventory(int ownerId)
        {
            return InventoriesMap[ownerId];
        }
    }
}
