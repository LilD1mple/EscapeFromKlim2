using EFK2.DialogSystem.Actions;
using EFK2.DialogSystem.Character;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFK2.DialogSystem.Scenes
{
	[CreateAssetMenu(fileName = "NewDialogScene", menuName = "Dialog System/New Dialog Scene", order = 58)]
	public class DialogScene : ScriptableObject
	{
		[Header("Sentences")]
		[SerializeField] private List<Sentence> _sentences;

		[Header("Next Scene")]
		[SerializeField] private DialogScene _nextStoryScene;

		public List<Sentence> Sentences => _sentences;

		public DialogScene NextStoryScene => _nextStoryScene;
	}

	[Serializable]
	public struct Sentence
	{
		public string text;
		public Speaker speaker;
		public List<SentenceActions> sentenceActions;
		public AudioClip sentenceClip;
	}

	[Serializable]
	public struct SentenceActions
	{
		public PersonActions personAction;
		public float duration;
	}
}