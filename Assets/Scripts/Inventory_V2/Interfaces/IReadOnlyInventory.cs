using System;

namespace Inventory
{
    public interface IReadOnlyInventory
    {
        public event Action<int, int> ItemAdded;
        public event Action<int, int> ItemRemoved;

        public int OwnerId { get; }

        public int GetAmount(int itemId);
        public int GetCapacity(int itemId);
        public bool Has(int itemId, int amount);
    }
}
