using TMPro;
using UnityEngine;

namespace EFK2.Settings.Audio
{
    public class AudioSettingViewer : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private TMP_Text _percentageText;

        public void RefreshUI(float value, float minValue, float maxValue)
        {
            _percentageText.text = $"{GetSliderPercentage(value, minValue, maxValue)}%";
        }

        private int GetSliderPercentage(in float value, in float minValue, in float maxValue)
        {
            float percentage = (value - minValue) / (maxValue - minValue) * 100f;

            return Mathf.CeilToInt(percentage);
        }
    }
}