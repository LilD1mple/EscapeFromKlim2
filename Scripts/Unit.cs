using UnityEngine;

namespace EFK2.Player
{
    public abstract class Unit : MonoBehaviour
    {
        protected abstract void HandleCharacterLook();

        protected abstract void HandleCharacterMovement();
    }
}