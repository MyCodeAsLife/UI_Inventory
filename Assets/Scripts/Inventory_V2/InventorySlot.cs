using System;

namespace Inventory
{
    public class InventorySlot : IReadOnlyInventorySlot
    {
        private readonly InventorySlotData Data;

        public event Action<int> ItemIdChanged;
        public event Action<int> ItemAmountChanged;

        public int ItemId
        {
            get => Data.ItemId;
            set
            {
                if (Data.ItemId != value)
                {
                    Data.ItemId = value;
                    ItemIdChanged?.Invoke(value);
                }
            }
        }
        public int Amount
        {
            get => Data.Amount;
            set
            {
                if (Data.Amount != value)
                {
                    Data.Amount = value;
                    ItemAmountChanged?.Invoke(value);
                }
            }
        }
        public bool IsEmpty => Data.Amount == 0 && Data.ItemId == 0;

        public InventorySlot(InventorySlotData data)
        {
            Data = data;
        }
    }
}
