using EFK2.Game.UpdateSystem.Interfaces;
using UnityEngine;

namespace EFK2.Player.Inventory.Items
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class HandItem : MonoBehaviour, IRunSystem
    {
        [Header("Local Position")]
        [SerializeField] private Vector3 _localPosition;
        [SerializeField] private Vector3 _localRotation;
        [SerializeField] private Vector3 _localScale;

        [Header("Throw")]
        [SerializeField] private float _throwForce;

        private Vector3 _startTransformScale;

        private Rigidbody _rigidbody;

        public bool IsPicked { get; protected set; } = false;

        public Vector3 LocalPosition => _localPosition;

        public Vector3 LocalRotation => _localRotation;   
        
        public Vector3 LocalScale => _localScale;

        public Vector3 StartTransformScale => _startTransformScale;

        public Rigidbody Rigidbody => _rigidbody;

        public float ThrowForce => _throwForce;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _startTransformScale = transform.lossyScale;

            OnStarted();
        }

        private void OnDestroy()
        {
            OnItemDestroyed();
        }

        void IRunSystem.Run()
        {
            OnSystemRun();
        }

        public virtual void OnItemPicked() { }

        public virtual void OnItemThrowed() { }

        protected virtual void OnItemInteract() { }

        protected virtual void OnItemDestroyed() { }

        protected virtual void OnStarted() { }

        protected virtual void OnSystemRun() { }
    }
}