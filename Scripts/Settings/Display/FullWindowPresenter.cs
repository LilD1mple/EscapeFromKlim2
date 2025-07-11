using EFK2.Game.ResetSystem;
using EFK2.Game.ScreenService.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings.Display
{
    public class FullWindowPresenter : MonoBehaviour, IResetable
    {
        [SerializeField] private Toggle _fullScreenToggle;

        private ResetService _resetService;

        private IScreenService _screenService;

        private void OnEnable()
        {
            _resetService.Register(this);

            _fullScreenToggle.onValueChanged.AddListener(ChangeScreenState);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);

            _fullScreenToggle.onValueChanged.RemoveListener(ChangeScreenState);
        }

        private void Start()
        {
            bool enabled = _screenService.FullScreen;

            _fullScreenToggle.isOn = enabled;

            ChangeScreenState(enabled);
        }

        [Inject]
        public void Construct(IScreenService screenService, ResetService resetService)
        {
            _screenService = screenService;

            _resetService = resetService;
        }

        private void ChangeScreenState(bool state)
        {
            _screenService.SetScreenState(state);
        }

        void IResetable.Reset()
        {
            _screenService.ResetScreenState(true);

            _fullScreenToggle.isOn = true;
        }
    }
}