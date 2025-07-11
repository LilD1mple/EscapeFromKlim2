using System;
using TMPro;
using UnityEngine;

namespace EFK2.Settings.InputSettings
{
    public class MouseSensivitySettingsViewer : MonoBehaviour
    {
        [Header("Source")]
        [SerializeField] private TMP_Text _sensivityText;

        public void RefreshUI(float value)
        {
            _sensivityText.text = $"{Math.Round(value, 1)}";
        }
    }
}