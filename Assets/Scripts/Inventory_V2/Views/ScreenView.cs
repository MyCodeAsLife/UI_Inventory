using UnityEngine;

namespace Inventory
{
    public class ScreenView : MonoBehaviour     // В чем смысл класса?
    {
        [SerializeField] private InventoryView _inventoryView;

        public InventoryView InventoryView => _inventoryView;
    }
}
