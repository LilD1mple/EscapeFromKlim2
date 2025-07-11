using UnityEngine;

namespace EFK2.DialogSystem.Character
{
	[CreateAssetMenu(fileName = "NewSpeaker", menuName = "Dialog System/New Speaker", order = 57)]
	public class Speaker : ScriptableObject
	{
		[SerializeField] private string _speakerName = "Клим";
		[SerializeField] private Sprite _speakerSprite;
		[SerializeField] private Color _textColor = Color.blue;

		public string SpeakerName => _speakerName;

		public Sprite SpeakerSprite => _speakerSprite;

		public Color TextColor => _textColor;
	}
}
