using EFK2.Game.FPSCounter.Interfaces;
using EFK2.Game.PauseSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace EFK2.UI.View
{
    public class FPSCounterView : MonoBehaviour, IPauseable
    {
        [Header("Source")]
        [SerializeField] private TMP_Text _fpsText;

        [Header("Settings")]
        [SerializeField, Min(0.001f)] private float _fpsRefreshRate;

        private float _timer = 0f;

        private bool _isPaused = false;

        private IFPSCounterService _fPSCounterService;

        private PauseService _pauseService;

        private const string _fpsTextConst = "{0} FPS";

        private void OnEnable()
        {
            _pauseService.Register(this);
        }

        private void OnDisable()
        {
            _pauseService.Unregister(this);
        }

        private void Update()
        {
            if (_isPaused || Time.unscaledTime < _timer)
                return;

            _timer = Time.unscaledTime + _fpsRefreshRate;

            OnFPSCountChanged(_fPSCounterService.GetFPSCount());
        }

        [Inject]
        public void Contruct(PauseService pauseService, IFPSCounterService fPSCounterService)
        {
            _fPSCounterService = fPSCounterService;

            _pauseService = pauseService;
        }

        private void OnFPSCountChanged(int count)
        {
            _fpsText.text = string.Format(_fpsTextConst, count);
        }

        void IPauseable.SetPause(bool isPaused)
        {
            _isPaused = isPaused;
        }
    }
}