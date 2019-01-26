using Game.Models;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class OrbitView : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        
        public void Bind(Orbit orbit)
        {
            if (_lineRenderer == null)
            {
                _lineRenderer = GetComponentInChildren<LineRenderer>();
            }

            var cnt = _lineRenderer.positionCount;
            var angleStep = (2 * Mathf.PI) / (cnt - 1);
            for (int i = 0; i < cnt; i++)
            {
                _lineRenderer.SetPosition(i, new Vector3(Mathf.Cos(i * angleStep) * orbit.Radius, 0f, Mathf.Sin(i * angleStep) * orbit.Radius));
            }
        }

        public class Pool : MonoMemoryPool<OrbitView> { }
    }
}