using EFK2.Extensions;
using EFK2.Player.Inventory;
using EFK2.Player.Inventory.Items;
using EFK2.Target.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Interact
{
    [RequireComponent(typeof(PlayerInventory))]
    public class RaycastService : MonoBehaviour, IRaycastService
    {
        [Header("Raycast Layer")]
        [SerializeField] private LayerMask _layerMask;

        [Header("Raycast Settings")]
        [SerializeField] private float _raycastDistance = 2f;
        [SerializeField] private float _raycastRadius = 0.02f;

        private Camera _raycastCamera;

        private IInventoryService _playerInventory;

        [Inject]
        public void Contruct(ITargetService playerTarget, IInventoryService playerInventory)
        {
            _playerInventory = playerInventory;

            _raycastCamera = playerTarget.PlayerController.MainCamera;
        }

        void IRaycastService.Raycast()
        {
            if (_raycastCamera.OverlapSphere(_raycastRadius, _raycastDistance, _layerMask, out var raycast))
            {
                if (raycast.TryFindComponent(out IInteractable interactable))
                    interactable.Interact();

                if (raycast.TryFindComponent(out HandItem inventoryItem))
                    _playerInventory.PickItem(inventoryItem);
            }
        }
    }
}