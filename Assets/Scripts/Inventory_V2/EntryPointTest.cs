using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    // ��� ������������
    public class EntryPoint : MonoBehaviour
    {
        private const int Owner_1 = 007;
        private const int Owner_2 = 1477;
        private readonly int[] _itemsId = new int[] { 11, 12, 13, 15 };

        [SerializeField] private ScreenView _screenView;

        private InventoryService _inventoryService;
        private ScreenController _screenController;
        private UserInputActions _userInputActions;

        private int _cachedOwnerId;

        private void Start()
        {
            _userInputActions = new UserInputActions();
            _userInputActions.Enable();
            _userInputActions.InventoryActions.OpenInventory1.performed += OpenInventory1;
            _userInputActions.InventoryActions.OpenInventory2.performed += OpenInventory2;
            _userInputActions.InventoryActions.AddItemsToInventory.performed += AddItemsToInventory;
            _userInputActions.InventoryActions.RemoveItemsFromInventory.performed += RemoveItemsFromInventory;

            // ��������� ������ ��������
            var gameStateProvider = new GameStatePlayerPrefsProvider();
            gameStateProvider.LoadGameState();                              // ������ ���� ����������� ��������

            _inventoryService = new InventoryService(gameStateProvider);
            var gameState = gameStateProvider.GameState;

            foreach (var inventoryData in gameState.Inventories)
            {
                _inventoryService.RegisterInventory(inventoryData);
            }

            _screenController = new ScreenController(_inventoryService, _screenView);
            _screenController.OpenInventory(Owner_1);
            _cachedOwnerId = Owner_1;       // ����� ����� ����� ��������� ������ � ������ ������
        }

        private void RemoveItemsFromInventory(InputAction.CallbackContext context)
        {
            int randIndex = Random.Range(0, _itemsId.Length);
            int randItemId = _itemsId[randIndex];
            int randAmount = Random.Range(0, 51);
            var result = _inventoryService.RemoveItems(_cachedOwnerId, randItemId, randAmount);

            Debug.Log($"Item remove: {randItemId}. Trying to remove: {result.ItemsRemoveAmount}. Success: {result.Success}.");
        }

        private void AddItemsToInventory(InputAction.CallbackContext context)
        {
            int randIndex = Random.Range(0, _itemsId.Length);
            int randItemId = _itemsId[randIndex];
            int randAmount = Random.Range(0, 51);
            var result = _inventoryService.AddItemsToInventory(_cachedOwnerId, randItemId, randAmount);

            Debug.Log($"Item added: {randItemId}. Amount added {result.ItemsAddedAmount}");
        }

        private void OpenInventory1(InputAction.CallbackContext context)
        {
            _screenController.OpenInventory(Owner_1);
            _cachedOwnerId = Owner_1;
        }

        private void OpenInventory2(InputAction.CallbackContext context)
        {
            _screenController.OpenInventory(Owner_2);
            _cachedOwnerId = Owner_2;
        }

        //private InventoryGridData CreateTestInventory(int ownerId)      // �������� �������� ���������
        //{
        //    var size = new Vector2Int(3, 4);                            // ������ ���������
        //    var createdInventorySlots = new List<InventorySlotData>();
        //    int length = size.x * size.y;   //+

        //    for (int i = 0; i < length; i++)
        //        createdInventorySlots.Add(new InventorySlotData());     // �������� ������ ������

        //    var createdInventoryData = new InventoryGridData            // �������� ���������
        //    {
        //        OwnerId = ownerId,                                      // ID ���������
        //        Size = size,                                            // ������ ���������
        //        Slots = createdInventorySlots,                          // ��������� ���� ������ ����� ��� ���������
        //    };

        //    return createdInventoryData;                                // ���������� ��������� ���������
        //}
    }
}