using EFK2.Game.Save;
using EFK2.Game.ScreenService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFK2.Game.ScreenService
{
    public sealed class ScreenService : IScreenService
    {
        private bool _isFullScreen;
        
        private int _resolutionIndex;

        private readonly Dictionary<string, (int, int)> _resolutions = new()
        {
            ["1280x720"] = (1280, 720),
            ["1920x1080"] = (1920, 1080),
            ["2560x1440"] = (2560, 1440),
            ["3840x2160"] = (3840, 2160)
        };

        private const bool _defaultScreenState = true;

        private const int _defaultResolutionIndex = 1;

        private const string _savedScreenState = "ScreenState";
        private const string _savedResolutionIndex = "Resolution";

        public ScreenService()
        {
            _isFullScreen = SaveUtility.LoadData(_savedScreenState, _defaultScreenState);

            _resolutionIndex = SaveUtility.LoadData(_savedResolutionIndex, _defaultResolutionIndex);
        }

        bool IScreenService.FullScreen => _isFullScreen;

        int IScreenService.ResolutionIndex => _resolutionIndex;

        void IScreenService.SetScreenResolution(int index)
        {
            AssingNewResolution(index);
        }

        void IScreenService.SetScreenState(bool state)
        {
            SetScreen(state);
        }

        void IScreenService.ResetScreenState(bool defaultEnable)
        {
            SaveUtility.DeleteKey(_savedScreenState);

            SetScreen(defaultEnable);
        }

        void IScreenService.ResetScreenResolution(int defaultResolutionIndex)
        {
            SaveUtility.DeleteKey(_savedResolutionIndex);

            AssingNewResolution(defaultResolutionIndex);
        }

        private void SetScreen(bool state)
        {
            _isFullScreen = state;

            Screen.fullScreen = state;

            SaveUtility.SaveData(_savedScreenState, state);
        }

        private void AssingNewResolution(int index)
        {
            if (index < 0 || index > _resolutions.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(index));

            (int, int) resolutions = _resolutions.ElementAt(index).Value;

            _resolutionIndex = index;

            Screen.SetResolution(resolutions.Item1, resolutions.Item2, _isFullScreen);

            SaveUtility.SaveData(_savedResolutionIndex, index);
        }
    }
}
