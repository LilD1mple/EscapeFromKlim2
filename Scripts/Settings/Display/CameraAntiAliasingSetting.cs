using EFK2.Game.ResetSystem;
using EFK2.Game.Save;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace EFK2.Settings.Display
{
    public class CameraAntiAliasingSetting : MonoBehaviour, IResetable
    {
        [SerializeField] private TMP_Dropdown _antiAliasingModes;
        [SerializeField] private UniversalAdditionalCameraData _cameraData;

        [Inject] private readonly ResetService _resetService;

        private readonly Dictionary<int, AntialiasingMode> _cameraAntiAliasingModes = new()
        {
            { 0, AntialiasingMode.None },
            { 1, AntialiasingMode.FastApproximateAntialiasing },
            { 2, AntialiasingMode.SubpixelMorphologicalAntiAliasing }
        };

        private const string _cameraAntiAliasing = "Anti-Aliasing";

        private void Start()
        {
            int index = SaveUtility.LoadData(_cameraAntiAliasing, 1);

            _antiAliasingModes.value = index;

            OnCameraModeChanged(index);
        }

        private void OnEnable()
        {
            _resetService.Register(this);

            _antiAliasingModes.onValueChanged.AddListener(OnCameraModeChanged);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);

            _antiAliasingModes.onValueChanged.RemoveListener(OnCameraModeChanged);
        }

        private void OnCameraModeChanged(int index)
        {
            _cameraData.antialiasing = _cameraAntiAliasingModes[index];

            SaveUtility.SaveData(_cameraAntiAliasing, index);
        }

        void IResetable.Reset()
        {
            SaveUtility.DeleteKey(_cameraAntiAliasing);

            _antiAliasingModes.value = 1;

            OnCameraModeChanged(1);
        }
    }
}
