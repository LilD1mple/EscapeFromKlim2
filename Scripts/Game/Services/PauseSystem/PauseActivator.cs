using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Inputs;
using EFK2.Inputs.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Game.PauseSystem
{
    public class PauseActivator : MonoBehaviour, IEventReceiver<PlayerDiedSignal>
    {
        private bool _isPaused = false;
        private bool _canShowPause = true;

        private PauseService _pauseService;
        private EventBus _eventBus;

        private IKeyboardInputService _keyboardInputService;

        UniqueId IBaseEventReceiver.Id => new();

        private void OnEnable()
        {
            _eventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            _eventBus.Unsubscribe(this);
        }

        private void Update()
        {
            if (_keyboardInputService.GetPressedKeyDown(InputConstants.pauseKey))
                RaisePause();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus == false && _isPaused == false)
                RaisePause();
        }

        [Inject]
        public void Construct(PauseService pauseService, EventBus eventBus, IKeyboardInputService keyboardInputService)
        {
            _pauseService = pauseService;

            _keyboardInputService = keyboardInputService;

            _eventBus = eventBus;
        }

        private void RaisePause()
        {
            if (_canShowPause == false)
                return;

            _isPaused = !_isPaused;

            _pauseService.SetPause(_isPaused);
        }

        void IEventReceiver<PlayerDiedSignal>.OnEvent(PlayerDiedSignal @event) => _canShowPause = false;
    }
}