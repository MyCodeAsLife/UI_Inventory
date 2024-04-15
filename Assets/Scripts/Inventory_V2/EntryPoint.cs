using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    // Для тестирования
    public class EntryPoint : MonoBehaviour
    {
        public InventoryGridView _view;

        private InventoryService _inventoryService = new();

        private void Start()
        {
            int ownerId = 1477;
            var inventoryData = CreateTestInventory(ownerId);
            var inventory = _inventoryService.RegisterInventory(inventoryData);
            _view.Setup(inventory);

            // Заполнить Slots (все упирается в нулевой capacity, так как он нигде не заполняется)
            // ID у всех одинаковый потому как capacity не вмещяет ничего и первый предмет просто занимает все слоты.
            // Добавить capacity при добавлении предмета
            //int count = 0;
            //for (int i = 0; i < inventory.Size.x; i++)
            //{
            //    for (int j = 0; j < inventory.Size.y; j++)
            //    {
            //        var position = new Vector2Int(i, j);
            //        inventory.AddItems(position, count, count + i);
            //        count++;
            //    }
            //}


            var addedResult = _inventoryService.AddItemsToInventory(ownerId, 2211, 15);
            Debug.Log($"Items added. ItemId: 2211, amount to add: 15, amount added: {addedResult.ItemsAddedAmount}");

            addedResult = _inventoryService.AddItemsToInventory(ownerId, 1111, 120);
            Debug.Log($"Items added. ItemId: 1111, amount to add: 120, amount added: {addedResult.ItemsAddedAmount}");

            addedResult = _inventoryService.AddItemsToInventory(ownerId, 1000, 40);
            Debug.Log($"Items added. ItemId: 1000, amount to add: 40, amount added: {addedResult.ItemsAddedAmount}");

            addedResult = _inventoryService.AddItemsToInventory(ownerId, 2211, 90);
            Debug.Log($"Items added. ItemId: 2211, amount to add: 90, amount added: {addedResult.ItemsAddedAmount}");

            addedResult = _inventoryService.AddItemsToInventory(ownerId, 999, 9999);
            Debug.Log($"Items added. ItemId: 999, amount to add: 9999, amount added: {addedResult.ItemsAddedAmount}");

            addedResult = _inventoryService.AddItemsToInventory(ownerId, 1345, 30);
            Debug.Log($"Items added. ItemId: 1345, amount to add: 30, amount added: {addedResult.ItemsAddedAmount}");

            var removedResult = _inventoryService.RemoveItems(ownerId, 1345, 30);
            Debug.Log($"Items added. ItemId: 1345, amount to remove: 30, amount Success: {removedResult.Success}");

            removedResult = _inventoryService.RemoveItems(ownerId, 1000, 70);
            Debug.Log($"Items added. ItemId: 1000, amount to remove: 70, amount Success: {removedResult.Success}");

            removedResult = _inventoryService.RemoveItems(ownerId, 999, 500);
            Debug.Log($"Items added. ItemId: 999, amount to remove: 500, amount Success: {removedResult.Success}");

            _view.Print();
        }

        private InventoryGridData CreateTestInventory(int ownerId)
        {
            var size = new Vector2Int(3, 4);
            var createdInventorySlots = new List<InventorySlotData>();
            int length = size.x * size.y;

            for (int i = 0; i < length; i++)
                createdInventorySlots.Add(new InventorySlotData());

            var createdInventoryData = new InventoryGridData
            {
                OwnerId = ownerId,
                Size = size,
                Slots = createdInventorySlots,
            };

            return createdInventoryData;
        }
    }
}