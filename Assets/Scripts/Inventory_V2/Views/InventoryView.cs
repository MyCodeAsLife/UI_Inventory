using TMPro;
using UnityEngine;

namespace Inventory
{
    public class InventoryView : MonoBehaviour          // Переименовать в InventoryGridView
    {
        [SerializeField] private InventorySlotView[] _slots;
        [SerializeField] private TMP_Text _textOwnerId;

        public string OwnerId
        {
            get => _textOwnerId.text;
            set => _textOwnerId.text = value;
        }

        public InventorySlotView GetInventorySlotView(int index)
        {
            return _slots[index];
        }
    }
}
