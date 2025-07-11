using UnityEngine;

namespace EFK2.Player
{
    public class TargetPointPlacer : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Transform _origin;

        [Header("Collision")]
        [SerializeField] private LayerMask _searchLayer;
        [SerializeField] private float _maxRaycastDownMagnitude;

        private void FixedUpdate()
        {
            RaycastDown();
        }

        private void RaycastDown()
        {
            if (Physics.Raycast(_origin.position, Vector3.down, out var hitInfo, _maxRaycastDownMagnitude, _searchLayer))
                transform.position = hitInfo.point;
        }
    }
}