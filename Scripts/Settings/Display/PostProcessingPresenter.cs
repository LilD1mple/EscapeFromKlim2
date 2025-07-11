using EFK2.Game.PostProcess.Interfaces;
using EFK2.Game.ResetSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings.Display
{
    public class PostProcessingPresenter : MonoBehaviour, IResetable
    {
        [SerializeField] private Toggle _postProcessToggle;

        private ResetService _resetService;

        private IPostProcessService _postProcessService;

        private void Start()
        {
            bool enabled = _postProcessService.IsEnable;

            _postProcessToggle.isOn = enabled;

            SetPostProcessState(enabled);
        }

        private void OnEnable()
        {
            _resetService.Register(this);

            _postProcessToggle.onValueChanged.AddListener(SetPostProcessState);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);

            _postProcessToggle.onValueChanged.RemoveListener(SetPostProcessState);
        }

        [Inject]
        public void Construct(IPostProcessService postProcessService, ResetService resetService)
        {
            _postProcessService = postProcessService;

            _resetService = resetService;
        }

        private void SetPostProcessState(bool enabled)
        {
            _postProcessService.SetPostProcessEnable(enabled);
        }

        void IResetable.Reset()
        {
            _postProcessService.Reset();

            _postProcessToggle.isOn = true;
        }
    }
}
