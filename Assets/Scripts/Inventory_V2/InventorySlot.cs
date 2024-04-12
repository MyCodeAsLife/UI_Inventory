using System;

namespace Inventory
{
    public class InventorySlot : IReadOnlyInventorySlot
    {
        private readonly InventorySlotData Data;

        public event Action<int> ItemIdChanged;
        public event Action<int> ItemAmountChanged;
        public event Action<int> SlotCapacityChanged;   // Можно добавить ивент на увеличение емкости\вместимости ячейки\слота\стака

        public int ItemId
        {
            get => Data.ItemId;
            set
            {
                if (Data.ItemId != value && value >= 0)
                {
                    Data.ItemId = value;
                    ItemIdChanged?.Invoke(value);
                }
            }
        }       // Вынести валидацию в InventoryGgrid?? (value > 0)
        public int Amount
        {
            get => Data.Amount;
            set
            {
                if (Data.Amount != value && value > 0)
                {
                    Data.Amount = value;
                    ItemAmountChanged?.Invoke(value);
                }
            }
        }       // Вынести валидацию в InventoryGgrid?? (value > 0)
        public int Capacity
        {
            get => Data.Capacity;
            set
            {
                if (Data.Capacity != value && value > 0)
                {
                    Data.Capacity = value;
                    SlotCapacityChanged?.Invoke(value);
                }
            }
        }     // Вынести валидацию в InventoryGgrid?? (value > 0)
        public bool IsEmpty => Data.Amount == 0 && Data.ItemId == 0;

        public InventorySlot(InventorySlotData data)
        {
            Data = data;
        }
    }
}
