using EFK2.Inputs.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.UI
{
    public class MouseVisibleSwitcher : MonoBehaviour
    {
        [Inject] private readonly IMouseInputService _mouseInputService;

        private void Awake()
        {
            _mouseInputService.SetCursorState(true);
        }
    }
}