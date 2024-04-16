using System;
using TMPro;
using UnityEngine;

namespace Inventory
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textTitleId;
        [SerializeField] private TMP_Text _textAmount;

        public string TitleId
        {
            get => _textTitleId.text;
            set => _textTitleId.text = value;
        }

        public string Amount
        {
            get => _textAmount.text;
            set => _textAmount.text = value;
        }
    }
}
