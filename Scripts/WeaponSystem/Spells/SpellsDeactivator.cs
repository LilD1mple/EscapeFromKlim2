using EFK2.Difficult;
using EFK2.UI.View;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.WeaponSystem.Spells
{
	public class SpellsDeactivator : MonoBehaviour
	{
		[SerializeField] private List<SpellData> _spells;

		[Inject]
		public void Construct(IDifficultService difficultService)
		{
			if (difficultService.DifficultConfiguration.EnableSpecialSkills == false)
				DeactivateSpells();
		}

		private void DeactivateSpells()
		{
			for (int i = 0; i < _spells.Count; i++)
			{
				_spells[i].spellView.enabled = false;

				_spells[i].spellPresenter.enabled = false;

				_spells[i].spellIcon.gameObject.SetActive(false);
			}
		}
	}

	[Serializable]
	public class SpellData
	{
		public SpellPresenter spellPresenter;
		public SpellView spellView;
		public Image spellIcon;
	}
}