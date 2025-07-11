using EFK2.UI.View;
using UnityEngine;

namespace EFK2.Player.Inventory.Items
{
	[RequireComponent(typeof(HandItemGUIView))]
	public class DoorKeyItem : HandItem
	{
		private HandItemGUIView _inventoryItemView;

		public override void OnItemPicked()
		{
			IsPicked = true;

			_inventoryItemView.SetActive(false);
		}

		public override void OnItemThrowed()
		{
			IsPicked = false;

			_inventoryItemView.SetActive(true);
		}

		protected override void OnStarted()
		{
			_inventoryItemView = GetComponent<HandItemGUIView>();
		}
	}
}
