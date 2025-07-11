using UnityEngine;
using Zenject;

namespace NTC.Pool
{
    public class NightPoolInjecter : MonoBehaviour
    {
        [Inject]
        public void Construct(DiContainer container) => NightPool.InjectContainer(container);
    }
}