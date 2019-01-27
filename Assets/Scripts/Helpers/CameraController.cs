using UnityEngine;

namespace Helpers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        private const float Speed = 5f;

        private void Update()
        {
            var p = _target.position;
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(p.x, transform.position.y, p.z),
                Time.deltaTime * Speed);
        }
    }
}
