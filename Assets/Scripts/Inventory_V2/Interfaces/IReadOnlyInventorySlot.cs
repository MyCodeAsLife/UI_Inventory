using System;

namespace Inventory
{
    public interface IReadOnlyInventorySlot
    {
        public event Action<int> ItemIdChanged;
        public event Action<int> ItemAmountChanged;

        public int ItemId { get; }
        public int Amount { get; }
        public bool IsEmpty { get; }
    }
}
