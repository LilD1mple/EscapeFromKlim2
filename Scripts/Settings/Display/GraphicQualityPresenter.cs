using EFK2.Game.ResetSystem;
using EFK2.Game.Save;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings
{
    public class GraphicQualityPresenter : MonoBehaviour, IResetable
    {
        [SerializeField] private Toggle[] _qualityToggles;

        [Inject] private readonly ResetService _resetService;

        private const string _qulityLevelIndexConst = "Quality";

        private void Start()
        {
            int index = SaveUtility.LoadData(_qulityLevelIndexConst, 2);

            _qualityToggles[index].isOn = true;

            SetQuality(index);
        }

        private void OnEnable()
        {
            _resetService.Register(this);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);
        }

        public void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);

            SaveUtility.SaveData(_qulityLevelIndexConst, index);
        }

        void IResetable.Reset()
        {
            SaveUtility.DeleteKey(_qulityLevelIndexConst);

            _qualityToggles[2].isOn = true;

            SetQuality(2);
        }
    }
}
