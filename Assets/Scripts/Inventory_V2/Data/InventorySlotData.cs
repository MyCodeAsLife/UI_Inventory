using System;

namespace Inventory
{
    [Serializable]
    public class InventorySlotData
    {
        public int ItemId;          // private ??
        public int Amount;          // private ??
        public int Capacity = 100;
    }
}