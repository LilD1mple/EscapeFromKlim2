using EFK2.Player.Inventory;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
    public class PlayerInventoryServiceInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInventory _playerInventory;

        public override void InstallBindings()
        {
            BindInventoryService();
        }

        private void BindInventoryService()
        {
            Container
                .Bind<IInventoryService>()
                .To<PlayerInventory>()
                .FromInstance(_playerInventory)
                .AsSingle();
        }
    } 
}