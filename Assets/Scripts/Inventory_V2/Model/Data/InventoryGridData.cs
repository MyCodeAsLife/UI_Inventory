using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class InventoryGridData
    {
        public int OwnerId;                     //private ??
        public List<InventorySlotData> Slots;   //private ??
        public Vector2Int Size;                 //private ??
    }
}
