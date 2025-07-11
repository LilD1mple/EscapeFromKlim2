using UnityEngine;
using UnityEngine.UI;

namespace EFK2.UI
{
    public class ApplicationExiter : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Application.Quit();
        }
    }
}