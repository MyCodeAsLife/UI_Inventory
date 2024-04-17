using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryGrid : IReadOnlyInventoryGrid
    {
        private readonly InventoryGridData Data;
        private readonly Dictionary<Vector2Int, InventorySlot> SlotsMap;        //+ Можно заменить на обычный двумерный массив

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
            SlotsMap = new();

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

        // Сюда добавить методы на изменение есмкости ячеек\слотов\стаков ???

        public AddItemsToInventoryGridResult AddItems(int itemId, int amount)
        {
            int remainingAmount = amount;
            int itemsAddedToSlotsWithSameItemsAmount = AddToSlotsWithSameItems(itemId, remainingAmount, out remainingAmount);

            if (remainingAmount < 1)
                return new AddItemsToInventoryGridResult(OwnerId, amount, itemsAddedToSlotsWithSameItemsAmount);

            int itemsAddedToAvailableSlotsAmount = AddToFirstAvailableSlots(itemId, remainingAmount, out remainingAmount);
            int totalAddedItemsAmount = itemsAddedToAvailableSlotsAmount + itemsAddedToSlotsWithSameItemsAmount;

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
            if (Has(itemId, amount) == false)
                return new RemoveItemsFromInventoryGridResult(OwnerId, amount, false);

            var amountToRemove = amount;

            for (int i = 0; i < Size.x; ++i)
            {
                for (int j = 0; j < Size.y; ++j)
                {
                    var slotCoords = new Vector2Int(i, j);  //+
                    var slot = SlotsMap[slotCoords];        //+

                    if (slot.ItemId != itemId)
                        continue;

                    if (amountToRemove > slot.Amount)
                    {
                        amountToRemove -= slot.Amount;
                        RemoveItems(slotCoords, itemId, slot.Amount);          //++ Не проще ли передать сразу ссылку слот, чем передавать структуру slotCoords?
                    }
                    else
                    {
                        RemoveItems(slotCoords, itemId, amountToRemove);       //++ Не проще ли передать сразу ссылку слот, чем передавать структуру slotCoords?
                        return new RemoveItemsFromInventoryGridResult(OwnerId, amount, true);
                    }
                }
            }

            throw new Exception("Something went wrong, couldn't remove some items.");
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

        public bool Has(int itemId, int amount) => GetAmount(itemId) >= amount;

        public int GetAmount(int itemId)
        {
            int amount = 0;

            foreach (var slot in Data.Slots)
                if (slot.ItemId == itemId)
                    amount += slot.Amount;

            return amount;
        }

        public int GetCapacity(int itemId)                      // Добавлять проверку на -1 в методы обращающиеся сюда
        {
            for (int i = 0; i < Data.Size.x; i++)
            {
                for (int j = 0; j < Data.Size.y; j++)
                {
                    Vector2Int position = new Vector2Int(i, j);

                    if (SlotsMap[position].ItemId == itemId)
                        return SlotsMap[position].Capacity;
                }
            }

            return -1;
        }

        public void SwitchSLots(Vector2Int slotCoordsA, Vector2Int slotCoordsB)
        {
            // Проверить работоспособность данного варианта
            //var temp = SlotsMap[slotCoordsA];
            //SlotsMap[slotCoordsA] = SlotsMap[slotCoordsB];
            //SlotsMap[slotCoordsB] = temp;

            var slotA = SlotsMap[slotCoordsA];
            var slotB = SlotsMap[slotCoordsB];
            int tempSlotItemId = slotA.ItemId;
            int tempSlotItemAmount = slotA.ItemId;
            slotA.ItemId = slotB.ItemId;
            slotA.Amount = slotB.Amount;
            slotB.ItemId = tempSlotItemId;
            slotB.Amount = tempSlotItemAmount;
        }

        public void SetSize(Vector2Int newSize)     // Изменение размеров инвентаря - изменение кол-ва ячеек/слотов инвентаря
        {
            // Создаем новый пустой инвентарь указанного размера
            // Перекладываем айтемы из старого инвентаря в новый
            // Обрабатываем ситуацию когда в новом инвентаре меньше ячеек чем было в старом, и все айтемы не влазиют
            throw new NotImplementedException();
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