using EFK2.Game.ResetSystem;
using EFK2.Game.ScreenService.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace EFK2.Settings.Display
{
    public class ScreenResolutionService : MonoBehaviour, IResetable
    {
        [SerializeField] private TMP_Dropdown _screenResolutionsDropdown;

        private ResetService _resetService;

        private IScreenService _screenService;

        private void Start()
        {
            int index = _screenService.ResolutionIndex;

            _screenResolutionsDropdown.value = index;

            SetResolution(index);
        }

        private void OnEnable()
        {
            _resetService.Register(this);

            _screenResolutionsDropdown.onValueChanged.AddListener(SetResolution);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);

            _screenResolutionsDropdown.onValueChanged.RemoveListener(SetResolution);
        }

        [Inject]
        public void Construct(IScreenService screenService, ResetService resetService)
        {
            _screenService = screenService;

            _resetService = resetService;
        }

        private void SetResolution(int index)
        {
            _screenService.SetScreenResolution(index);
        }

        void IResetable.Reset()
        {
            _screenService.ResetScreenResolution(1);

            _screenResolutionsDropdown.value = 1;
        }
    }
}
