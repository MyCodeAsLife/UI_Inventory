using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryGrid : IReadOnlyInventoryGrid
    {
        private readonly InventoryGridData Data;
        private readonly Dictionary<Vector2Int, InventorySlot> SlotsMap = new();        //+ Можно заменить на обычный двумерный массив

        public event Action<int, int> ItemAdded;
        public event Action<int, int> ItemRemoved;
        public event Action<Vector2Int> SizeChanged;

        public int OwnerId => Data.OwnerId;
        public Vector2Int Size      // Добавить проверку на value < 0 ???
        {
            get => Data.Size;
            set
            {
                if (Data.Size != value)
                {
                    Data.Size = value;
                    SizeChanged?.Invoke(value);
                }
            }
        }

        public InventoryGrid(InventoryGridData data)
        {
            Data = data;

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    int index = i * Size.y + j;            //+ Конвертируем индексацию двумерного массива в одномерный
                    var slotData = Data.Slots[index];           //+
                    var slot = new InventorySlot(slotData);     //+ Заваричиваем данные слота в объект слота
                    var position = new Vector2Int(i, j);        //+

                    SlotsMap[position] = slot;                  // Если такого ключа не существует, то создает пару с таким ключом
                }
            }
        }

        public IReadOnlyInventorySlot[,] GetSlots()
        {
            var array = new IReadOnlyInventorySlot[Size.x, Size.y];

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    var position = new Vector2Int(i, j);
                    array[i, j] = SlotsMap[position];
                }
            }

            return array;
        }

        public bool Has(int itemId, int amount)                 ///
        {
            throw new NotImplementedException();
        }

        // Сюда добавить методы на изменение есмкости ячеек\слотов\стаков ???

        public AddItemsToInventoryGridResult AddItems(int itemId, int amount)
        {
            int remainingAmount = amount;
            int itemsAddedToSlotsWithSameItemsAmount = AddToSlotsWithSameItems(itemId, remainingAmount, out remainingAmount);

            if (remainingAmount < 1)
                return new AddItemsToInventoryGridResult(OwnerId, amount, itemsAddedToSlotsWithSameItemsAmount);

            int itemsAddedToAvailableSlotsAmount = AddToFirstAvailableSlots(itemId, remainingAmount, out remainingAmount);
            int totalAddedItemsAmount = itemsAddedToSlotsWithSameItemsAmount + itemsAddedToSlotsWithSameItemsAmount;

            return new AddItemsToInventoryGridResult(OwnerId, amount, totalAddedItemsAmount);
        }

        public AddItemsToInventoryGridResult AddItems(Vector2Int slotCoords, int itemId, int amount)     /// Вынести сюда валидацию из InventorySlot??? (value > 0) и валидацию координат слота???
        {
            // Перенести сюда валидацию itemId, amount и slotCoords ???

            var slot = SlotsMap[slotCoords];            //+
            int newValue = slot.Amount + amount;        // Подсчитываем общее кол-во айтемов, то што добавляют + то что уже есть в ячейке
            int itemsAddedAmount = 0;

            if (slot.IsEmpty)
                slot.ItemId = itemId;

            var itemSlotCapacity = slot.Capacity;       //+

            if (newValue > itemSlotCapacity)
            {
                int remainingItems = newValue - itemSlotCapacity;
                int itemsToAddAmount = itemSlotCapacity - slot.Amount;
                itemsAddedAmount += itemsToAddAmount;
                slot.Amount = itemSlotCapacity;

                var result = AddItems(itemId, remainingItems);
                itemsAddedAmount += result.ItemsAddedAmount;
            }
            else
            {
                itemsAddedAmount = amount;
                slot.Amount = newValue;
            }

            return new AddItemsToInventoryGridResult(OwnerId, amount, itemsAddedAmount);
        }

        public RemoveItemsFromInventoryGridResult RemoveItems(int itemId, int amount)                 /// Остановился здесь---------------------------
        {
            throw new NotImplementedException();
        }

        public RemoveItemsFromInventoryGridResult RemoveItems(Vector2Int slotCoords, int itemId, int amount)
        {
            var slot = SlotsMap[slotCoords];    //+

            if (slot.IsEmpty || slot.ItemId != itemId || slot.Amount < amount)
                return new RemoveItemsFromInventoryGridResult(OwnerId, amount, false);

            slot.Amount -= amount;

            if (slot.Amount == 0)
                slot.ItemId = 0;

            return new RemoveItemsFromInventoryGridResult(OwnerId, amount, true);
        }

        public int GetAmount(int itemId)                        /// Добавлять проверку на -1 в методы обращающиеся сюда
        {
            if (GetItemSlotPosition(itemId, out Vector2Int position))
                return SlotsMap[position].Amount;

            return -1;
        }

        public int GetCapacity(int itemId)                      // Добавлять проверку на -1 в методы обращающиеся сюда
        {
            if (GetItemSlotPosition(itemId, out Vector2Int position))
                return SlotsMap[position].Capacity;

            return -1;
        }

        private bool GetItemSlotPosition(int itemId, out Vector2Int resultPosition)
        {
            resultPosition = default(Vector2Int);

            for (int i = 0; i < Data.Size.x; i++)
            {
                for (int j = 0; j < Data.Size.y; j++)
                {
                    Vector2Int position = new Vector2Int(i, j);

                    if (SlotsMap[position].ItemId == itemId)
                    {
                        resultPosition = position;
                        return true;
                    }
                }
            }

            return false;
        }

        private int AddToSlotsWithSameItems(int itemId, int amount, out int remainingAmount)
        {
            int itemsAddedAmount = 0;
            remainingAmount = amount;

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    Vector2Int position = new Vector2Int(i, j);

                    if (SlotsMap[position].IsEmpty)
                        continue;

                    int slotItemCapacity = SlotsMap[position].Capacity;                     //+ Не стыковка с видео (мой вариант)
                    //int slotItemCapacity = GetCapacity(SlotsMap[position].ItemId);        // Не стыковка с видео (вариант с видео)

                    if (SlotsMap[position].Amount >= slotItemCapacity)
                        continue;

                    if (SlotsMap[position].ItemId != itemId)
                        continue;

                    int newValue = SlotsMap[position].Amount + remainingAmount;

                    if (newValue > slotItemCapacity)
                    {
                        remainingAmount = newValue - slotItemCapacity;
                        int itemsToAddAmount = slotItemCapacity - SlotsMap[position].Amount;    //+
                        itemsAddedAmount += itemsToAddAmount;                                   //+
                        SlotsMap[position].Amount = slotItemCapacity;

                        if (remainingAmount == 0)
                            return itemsAddedAmount;
                    }
                    else
                    {
                        itemsAddedAmount += remainingAmount;
                        SlotsMap[position].Amount = newValue;
                        remainingAmount = 0;

                        return itemsAddedAmount;
                    }
                }
            }

            return itemsAddedAmount;
        }

        private int AddToFirstAvailableSlots(int itemId, int amount, out int remainingAmount)    ///
        {
            var itemsAddedAmount = 0;
            remainingAmount = amount;

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    Vector2Int position = new Vector2Int(i, j);

                    if (!SlotsMap[position].IsEmpty)
                        continue;

                    SlotsMap[position].ItemId = itemId;
                    int newValue = remainingAmount;

                    if (newValue > SlotsMap[position].Capacity)
                    {
                        remainingAmount = newValue - SlotsMap[position].Capacity;
                        int itemsToAddAmount = SlotsMap[position].Capacity;         //+
                        itemsAddedAmount += itemsToAddAmount;                       //+
                        SlotsMap[position].Amount = SlotsMap[position].Capacity;
                    }
                    else
                    {
                        itemsAddedAmount += remainingAmount;
                        SlotsMap[position].Amount = newValue;
                        remainingAmount = 0;

                        return itemsAddedAmount;
                    }
                }
            }

            return itemsAddedAmount;
        }
    }
}