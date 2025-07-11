using EFK2.Inputs;
using EFK2.Inputs.Interfaces;
using EFK2.Player.Inventory.Items;
using EFK2.Target.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Player.Inventory
{
    public class PlayerInventory : MonoBehaviour, IInventoryService
    {
        [Header("Item Muzzle")]
        [SerializeField] private Transform _itemHolder;

        private Camera _mainCamera;

        private HandItem _inventoryItem;

        private IKeyboardInputService _keyboardInput;

        public bool IsHasItem => _inventoryItem != null;

        HandItem IInventoryService.CurrentItem => _inventoryItem;

        private void Update()
        {
            if (_keyboardInput.GetPressedKey(InputConstants.throwKeyConst) && _inventoryItem != null)
                ThrowItem();
        }

        [Inject]
        public void Construct(ITargetService playerTarget, IKeyboardInputService keyboardInputService)
        {
            _mainCamera = playerTarget.PlayerController.MainCamera;

            _keyboardInput = keyboardInputService;
        }

        public void ThrowItem()
        {
            _inventoryItem.transform.parent = null;

            _inventoryItem.transform.localScale = _inventoryItem.StartTransformScale;

            ThrowInventoryItem(_inventoryItem);

            _inventoryItem?.OnItemThrowed();

            _inventoryItem = null;
        }

        private void SetItemLocalTransform(HandItem item)
        {
            item.transform.parent = _itemHolder;

            item.transform.localPosition = item.LocalPosition;

            item.transform.localEulerAngles = item.LocalRotation;

            item.transform.localScale = item.LocalScale;

            item.Rigidbody.isKinematic = true;

            item.Rigidbody.detectCollisions = false;
        }

        private void ThrowInventoryItem(HandItem item)
        {
            item.Rigidbody.isKinematic = false;

            item.Rigidbody.detectCollisions = true;

            item.Rigidbody.AddForce(_mainCamera.transform.forward * item.ThrowForce, ForceMode.Impulse);
        }

        void IInventoryService.PickItem(HandItem item)
        {
            if (_inventoryItem != null)
                ThrowItem();

            SetItemLocalTransform(item);

            item.OnItemPicked();

            _inventoryItem = item;
        }
    }
}