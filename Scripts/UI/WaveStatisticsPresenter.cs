using EFK2.Factory;
using TMPro;
using UnityEngine;

namespace EFK2.UI
{
    public class WaveStatisticsPresenter : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private EnemyFactory _enemyFactory;

        [Header("Info View")]
        [SerializeField] private TMP_Text _waveNumberText;
        [SerializeField] private TMP_Text _remainigEnemiesText;

        private void OnEnable()
        {
            _enemyFactory.WaveChanged += OnWaveChanged;

            _enemyFactory.EnemiesCountChanged += OnEnemiesCountChanged;
        }

        private void OnDisable()
        {
            _enemyFactory.EnemiesCountChanged -= OnEnemiesCountChanged;

            _enemyFactory.WaveChanged -= OnWaveChanged;
        }

        private void OnWaveChanged(int waveNumber)
        {
            _waveNumberText.text = $"Волна {waveNumber}";
        }

        private void OnEnemiesCountChanged(int remainingEnemies)
        {
            _remainigEnemiesText.text = remainingEnemies.ToString();
        }
    }
}