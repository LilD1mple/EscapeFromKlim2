using UnityEngine;

namespace EFK2.Player.Inventory.Items
{
    public class ItemHandHolder : MonoBehaviour
    {
        [Header("Hands Holder")]
        [SerializeField] private bool _enabled = true;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerInventory _playerInventory;

        [Space, Header("Main")]
        [SerializeField, Range(0.0005f, 0.02f)] private float _amount = 0.005f;
        [SerializeField, Range(1f, 3f)] private float _sprintAmount = 1.4f;
        [SerializeField, Range(5f, 20f)] private float _frequency = 13f;
        [SerializeField, Range(50f, 10f)] private float _smooth = 24.2f;

        [Header("Rotation Movement")]
        [SerializeField] private bool _enabledRotationMovement = true;
        [SerializeField, Range(0.1f, 10f)] private float _rotationMultipler = 6f;

        private float _toggleSpeed = 1.5f;
        private float _amountValue;

        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private Vector3 _finalPosition;
        private Vector3 _finalRotation;

        private void Awake()
        {
            _amountValue = _amount;

            _startPosition = transform.localPosition;

            _startRotation = transform.localEulerAngles;
        }

        private void Update()
        {
            if (_enabled == false || _playerInventory.IsHasItem == false)
                return;

            float speed = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).magnitude;

            Reset();

            if (speed > _toggleSpeed && _characterController.isGrounded)
            {
                Vector3 headBobAmount = HeadBobMotion();

                _finalPosition += headBobAmount;

                _finalRotation += new Vector3(-headBobAmount.z, 0, headBobAmount.x) * _rotationMultipler * 10;
            }
            else if (speed > _toggleSpeed)
                _finalPosition += HeadBobMotion() / 2f;

            if (Input.GetKeyDown(KeyCode.LeftShift))
                _amountValue = _amount * _sprintAmount;
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                _amountValue = _amount / _sprintAmount;

            transform.localPosition = Vector3.Lerp(transform.localPosition, _finalPosition, _smooth * Time.deltaTime);

            if (_enabledRotationMovement)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(_finalRotation), _smooth / 1.5f * Time.deltaTime);
        }

        private Vector3 HeadBobMotion()
        {
            Vector3 pos = Vector3.zero;

            pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * _frequency) * _amountValue * 2f, _smooth * Time.deltaTime);
            pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * _frequency / 2f) * _amountValue * 1.3f, _smooth * Time.deltaTime);

            return pos;
        }

        private void Reset()
        {
            if (transform.localPosition == _startPosition)
                return;

            _finalPosition = Vector3.Lerp(_finalPosition, _startPosition, 1 * Time.deltaTime);

            _finalRotation = Vector3.Lerp(_finalRotation, _startRotation, 1 * Time.deltaTime);
        }
    }
}