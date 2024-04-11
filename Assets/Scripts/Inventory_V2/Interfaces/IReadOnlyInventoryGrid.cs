using System;
using UnityEngine;

namespace Inventory
{
    public interface IReadOnlyInventoryGrid : IReadOnlyInventory
    {
        public event Action<Vector2Int> SizeChanged;

        public Vector2Int Size { get; }

        public IReadOnlyInventorySlot[,] GetSlots();
    }
}