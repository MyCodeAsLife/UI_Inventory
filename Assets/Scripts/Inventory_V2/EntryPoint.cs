using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    // Для тестирования
    public class EntryPoint : MonoBehaviour
    {
        public InventoryGridView _view;

        private void Start()
        {
            var inventoryData = new InventoryGridData
            {
                Size = new Vector2Int(3, 4),
                OwnerId = 1477,
                Slots = new List<InventorySlotData>(),
            };

            // Заполнить Slots
            int count = 0;
            for (int i = 0; i < inventoryData.Size.x; i++)
            {
                for (int j = 0; j < inventoryData.Size.y; j++)
                {
                    var item = new InventorySlotData();
                    item.ItemId = count;
                    item.Amount = count + i;

                    inventoryData.Slots.Add(item);
                    count++;
                }
            }

            var slotData = inventoryData.Slots[0];
            slotData.ItemId = 1970;
            slotData.Amount = 10;

            var inventory = new InventoryGrid(inventoryData);

            _view.Setup(inventory);
        }
    }
}