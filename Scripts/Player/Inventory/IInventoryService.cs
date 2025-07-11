using EFK2.Player.Inventory.Items;

namespace EFK2.Player.Inventory
{
    public interface IInventoryService
    {
        HandItem CurrentItem { get; }

        void PickItem(HandItem item);

        void ThrowItem();
    }
}
