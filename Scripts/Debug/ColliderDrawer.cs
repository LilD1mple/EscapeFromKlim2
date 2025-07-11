using UnityEngine;

namespace EFK2.Debugs
{
	public class ColliderDrawer : MonoBehaviour
	{
		private Mesh _sphereMesh;
		private Mesh _capsuleMesh;
		private Mesh _cubeMesh;

		[SerializeField]
		private Material _colliderMaterial;

		private Collider[] _colliders;

		private KeyCode _renderKey = KeyCode.L;

		private bool _render = false;

		// little hack to get the default shape meshes
		// note that the normal collider Gizmos use actually more simple drawings but -as this is just for debugging this should be enough 
		private void Awake()
		{
			var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			_cubeMesh = cube.GetComponent<MeshFilter>().sharedMesh;

			var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			_sphereMesh = sphere.GetComponent<MeshFilter>().sharedMesh;

			var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			_capsuleMesh = capsule.GetComponent<MeshFilter>().sharedMesh;

			if (Application.isPlaying)
			{
				Destroy(sphere);
				Destroy(cube);
				Destroy(capsule);
			}
			else
			{
				DestroyImmediate(sphere);
				DestroyImmediate(cube);
				DestroyImmediate(capsule);
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(_renderKey))
			{
				_render = !_render;

				_colliders = FindObjectsOfType<Collider>();
			}

			if (_render)
				RenderColliders();
		}

		private void RenderColliders()
		{
			// then draw their meshes
			foreach (var collider in _colliders)
			{
				Mesh mesh;
				var colliderTransform = collider.transform;
				var matrix = colliderTransform.localToWorldMatrix;

				switch (collider)
				{
					case BoxCollider boxCollider:
						{
							// for a box we take the cube mesh and additionally translate and scale it according to the settings
							mesh = _cubeMesh;
							matrix *= Matrix4x4.TRS(boxCollider.center, Quaternion.identity, boxCollider.size);
							break;
						}

					case SphereCollider sphereCollider:
						{
							// sphere is almot the same but uses the maximum of the scales as radius 
							// then we add center and radius settings
							mesh = _sphereMesh;
							var lossyScale = colliderTransform.lossyScale;
							matrix = Matrix4x4.TRS(colliderTransform.position, colliderTransform.rotation, Vector3.one * Mathf.Max(lossyScale.x, lossyScale.y, lossyScale.z)) * Matrix4x4.TRS(sphereCollider.center, Quaternion.identity, 2 * sphereCollider.radius * Vector3.one);
							break;
						}

					case MeshCollider meshCollider:
						{
							// mesh is trivial
							mesh = meshCollider.sharedMesh;
							break;
						}

					case CapsuleCollider capsuleCollider:
						{
							// capsule is a bit complex as they actually use a sphere, cylinder combo but this comes close enough
							// might need some more work though
							mesh = _capsuleMesh;
							var radius = capsuleCollider.radius * 2;
							var height = capsuleCollider.height > radius ? capsuleCollider.height : radius;
							height *= 0.5f;
							var size = new Vector3(radius, height, radius);
							var rotation = capsuleCollider.direction switch
							{
								0 => Quaternion.Euler(0, 0, 90),
								1 => Quaternion.identity,
								2 => Quaternion.Euler(90, 0, 0)
							};
							matrix *= Matrix4x4.TRS(capsuleCollider.center, rotation, size);
							break;
						}

					default:
						continue;
				}

				Graphics.DrawMesh(mesh, matrix, _colliderMaterial, 0);
			}
		}
	}
}