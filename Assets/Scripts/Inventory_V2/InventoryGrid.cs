using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryGrid : IReadOnlyInventoryGrid
    {
        private readonly InventoryGridData Data;
        private readonly Dictionary<Vector2Int, InventorySlot> SlotsMap = new();        //+ ����� �������� �� ������� ��������� ������

        public event Action<int, int> ItemAdded;
        public event Action<int, int> ItemRemoved;
        public event Action<Vector2Int> SizeChanged;

        public int OwnerId => Data.OwnerId;
        public Vector2Int Size
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
            var size = Data.Size;

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    int index = i * size.y + j;                 //+ ������������ ���������� ���������� ������� � ����������
                    var slotData = Data.Slots[index];           //+
                    var slot = new InventorySlot(slotData);     //+ ������������ ������ ����� � ������ �����
                    var position = new Vector2Int(i, j);        //+

                    SlotsMap[position] = slot;                  // ���� ������ ����� �� ����������, �� ������� ���� � ����� ������
                }
            }
        }

        public int GetAmount(int itemId)                        ///
        {
            throw new NotImplementedException();
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

        public AddItemsToInventoryGridResult AddItems(int itemId, int amount)            ///
        {
            throw new NotImplementedException();
        }

        public AddItemsToInventoryGridResult AddItems(Vector2Int slotCoords, int itemId, int amount)     ///
        {
            throw new NotImplementedException();
        }

        public RemoveItemsFromInventoryGridResult RemoveItems(int itemId, int amount)                 ///
        {
            throw new NotImplementedException();
        }

        public RemoveItemsFromInventoryGridResult RemoveItems(Vector2Int slotCoords, int itemId, int amount)      ///
        {
            throw new NotImplementedException();
        }
    }
}