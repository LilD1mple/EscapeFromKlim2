using EFK2.Difficult;
using UnityEngine;
using Zenject;

namespace EFK2.UI
{
    public class LevelDifficultSwitcher : MonoBehaviour
    {
        [Inject] private readonly IDifficultService _difficultSerivce;

        public void SetDiffcultLevel(DifficultLevelConfiguration configuration) => _difficultSerivce.SetLevelDifficult(configuration);
    }
}