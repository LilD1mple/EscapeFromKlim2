using EFK2.Game.UpdateSystem;
using EFK2.Inputs;
using EFK2.Inputs.Interfaces;
using EFK2.UI.View;
using EFK2.WeaponSystem;
using UnityEngine;
using Zenject;

namespace EFK2.Player.Inventory.Items
{
	[RequireComponent(typeof(HandItemGUIView))]
	public class CrossItem : HandItem
	{
		[Header("Custom fields")]
		[SerializeField] private ProjectileFactory _crossAttack;

		private GlobalUpdate _globalUpdate;

		private IKeyboardInputService _keyboardInput;

		private HandItemGUIView _inventoryItemView;

		[Inject]
		public void Construct(GlobalUpdate globalUpdate, IKeyboardInputService keyboardInputService)
		{
			_globalUpdate = globalUpdate;

			_keyboardInput = keyboardInputService;
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
			if (_keyboardInput.GetPressedKey(InputConstants.attackKeyConst))
				_crossAttack.PerformAttack();
		}

		protected override void OnItemDestroyed()
		{
			_globalUpdate.UnregistRunSystem(this);
		}
	}
}
