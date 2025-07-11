using EFK2.Game.UpdateSystem;
using EFK2.Inputs;
using EFK2.Inputs.Interfaces;
using EFK2.UI.View;
using UnityEngine;
using Zenject;

namespace EFK2.Player.Inventory.Items
{
	[RequireComponent(typeof(HandItemGUIView))]
	public class FlashlightItem : HandItem
	{
		[Header("Custom fields")]
		[SerializeField] private Light _light;

		private GlobalUpdate _globalUpdate;
		private IKeyboardInputService _keyboardInputService;

		private bool _isLightEnabled = false;

		private HandItemGUIView _inventoryItemView;

		[Inject]
		public void Construct(GlobalUpdate globalUpdate, IKeyboardInputService keyboardInputService)
		{
			_globalUpdate = globalUpdate;

			_keyboardInputService = keyboardInputService;
		}

		protected override void OnItemInteract()
		{
			if (IsPicked == false)
				return;

			_isLightEnabled = !_isLightEnabled;

			_light.enabled = _isLightEnabled;
		}

		public override void OnItemPicked()
		{
			IsPicked = true;

			_globalUpdate.RegistRunSystem(this);

			_inventoryItemView.SetActive(false);
		}

		public override void OnItemThrowed()
		{
			IsPicked = false;

			_globalUpdate.UnregistRunSystem(this);

			_inventoryItemView.SetActive(true);
		}

		protected override void OnStarted()
		{
			_inventoryItemView = GetComponent<HandItemGUIView>();
		}

		protected override void OnSystemRun()
		{
			if (_keyboardInputService.GetPressedKey(InputConstants.flashlightKeyConst))
				OnItemInteract();
		}
	}
}