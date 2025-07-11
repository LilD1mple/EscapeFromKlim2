using UnityEngine;

namespace EFK2.Player.Inventory.Items
{
    public class ItemSway : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerInventory _playerInventory;

        [Header("HandsSmooth")]
        [SerializeField, Range(1, 10)] private float _smooth = 4f;
        [SerializeField, Range(0.001f, 1)] private float _amount = 0.03f;
        [SerializeField, Range(0.001f, 1)] private float _maxAmount = 0.04f;

        [Header("Rotation")]
        [SerializeField, Range(1, 10)] private float _rotationSmooth = 4.0f;
        [SerializeField, Range(0.1f, 10)] private float _RotationAmount = 1.0f;
        [SerializeField, Range(0.1f, 10)] private float _maxRotationAmount = 5.0f;
        [SerializeField, Range(0.1f, 10)] private float _rotationMovementMultipler = 1.0f;

        private Vector3 _startPosition;

        private Quaternion _startRotation;

        private float _croughRotation;

        private const string _mouseXAxis = "Mouse X";
        private const string _mouseYAxis = "Mouse Y";
        private const string _horizontalAxis = "Horizontal";
        private const string _verticalAxis = "Vertical";

        private void Start()
        {
            _startPosition = transform.localPosition;

            _startRotation = transform.localRotation;
        }

        private void Update()
        {
            if (_playerInventory.IsHasItem == false)
                return;

            UpdateHandsPosition();

            UpdateHandsRotation();
        }

        private void UpdateHandsPosition()
        {
            float InputX = -Input.GetAxis(_mouseXAxis);
            float InputY = -Input.GetAxis(_mouseYAxis);

            float moveX = Mathf.Clamp(InputX * _amount, -_maxAmount, _maxAmount);
            float moveY = Mathf.Clamp(InputY * _amount, -_maxAmount, _maxAmount);

            Vector3 finalPosition = new Vector3(moveX, moveY + -_characterController.velocity.y / 60, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + _startPosition, Time.deltaTime * _smooth);
        }

        private void UpdateHandsRotation()
        {
            float InputX = -Input.GetAxis(_mouseXAxis);
            float InputY = -Input.GetAxis(_mouseYAxis);

            float horizontal = -Input.GetAxis(_horizontalAxis);
            float vertical = Input.GetAxis(_verticalAxis);

            float TiltX = Mathf.Clamp(InputX * _RotationAmount, -_maxRotationAmount, _maxRotationAmount);
            float TiltY = Mathf.Clamp(InputY * _rotationSmooth, -_maxRotationAmount, _maxRotationAmount);

            Vector3 vector = new Vector3(Mathf.Max(vertical * 0.4f, 0) * _rotationMovementMultipler, 0, horizontal * _rotationMovementMultipler);

            Vector3 finalRotation = new Vector3(-TiltY, 0, TiltX + _croughRotation) + vector;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(finalRotation) * _startRotation, Time.deltaTime * _rotationSmooth);
        }
    } 
}