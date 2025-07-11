using EFK2.Game.ResetSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings
{
    public class ResetSettingsPresenter : MonoBehaviour
    {
        [SerializeField] private Button _resetButton;

        [Inject] private readonly ResetService _resetService;

        private void OnEnable()
        {
            _resetButton.onClick.AddListener(ResetSettings);
        }

        private void OnDisable()
        {
            _resetButton.onClick.RemoveListener(ResetSettings);
        }

        private void ResetSettings()
        {
            _resetService.Reset();
        }
    }
}