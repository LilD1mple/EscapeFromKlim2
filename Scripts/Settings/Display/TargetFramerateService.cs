using EFK2.Game.FPSCounter.Interfaces;
using EFK2.Game.ResetSystem;
using EFK2.Game.Save;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace EFK2.Settings.Display
{
    public class TargetFramerateService : MonoBehaviour, IResetable
    {
        [SerializeField] private TMP_Dropdown _targetFramerateChoices;

        private ResetService _resetService;

        private IFPSCounterService _fpsCounterService;

        private const string _savedTargetFramerate = "TargetFramerate";

        private readonly Dictionary<string, int> _options = new()
        {
            ["Без ограничения"] = -1,
            ["30"] = 30,
            ["60"] = 60,
            ["90"] = 90,
            ["120"] = 120,
            ["150"] = 150,
            ["180"] = 180,
            ["240"] = 240
        };

        private void Start()
        {
            int choice = SaveUtility.LoadData(_savedTargetFramerate, 0);

            _targetFramerateChoices.value = choice;

            ChangeTargetFramerateResolution(choice);
        }

        private void OnEnable()
        {
            _resetService.Register(this);

            _targetFramerateChoices.onValueChanged.AddListener(ChangeTargetFramerateResolution);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);

            _targetFramerateChoices.onValueChanged.RemoveListener(ChangeTargetFramerateResolution);
        }

        [Inject]
        public void Contruct(ResetService resetService, IFPSCounterService fPSCounterService)
        {
            _resetService = resetService;

            _fpsCounterService = fPSCounterService;
        }

        private void ChangeTargetFramerateResolution(int index)
        {
            string choice = _targetFramerateChoices.options[index].text;

            int value = _options[choice];

            _fpsCounterService.SetFPSLimit(value);

            Application.targetFrameRate = value;

            SaveUtility.SaveData(_savedTargetFramerate, index);
        }

        void IResetable.Reset()
        {
            SaveUtility.DeleteKey(_savedTargetFramerate);

            _targetFramerateChoices.value = 0;

            ChangeTargetFramerateResolution(0);
        }
    }
}