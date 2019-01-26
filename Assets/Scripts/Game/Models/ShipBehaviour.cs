using System;
using UnityEngine;
using Zenject;

namespace Game.Models
{
    public class ShipBehaviour : MonoBehaviour
    {
        private Ship _model;

        public event Action TargetReached;

        [Inject]
        private void Construct(Ship model)
        {
            _model = model;
        }
        
        //changed with planet later
        private PlanetBehaviour _targetPlanet;

        public void SetTarget(PlanetBehaviour target)
        {
            if (_model.State == ShipState.Idle)
            {
                _targetPlanet = target;
                _model.State = ShipState.Moving;
            }
        }

        private void Awake()
        {
            _model.State = ShipState.Idle;  
        }

        private void Update()
        {
            if (_targetPlanet != null)
            {
                //check is in range of the planet
                var t = _targetPlanet.transform;
                _model.CurrentSpeed = Mathf.Clamp(_model.CurrentSpeed + _model.Acceleration, 0f, _model.MaxMoveSpeed);

                var dir = (t.position - transform.position).normalized;
                var angleBetween = Vector3.Angle(transform.forward, dir);

                var rot =  angleBetween >= _model.RotationSpeed ? _model.RotationSpeed : angleBetween;
                rot *= Vector3.Dot(dir, transform.right)>0 ? 1 : -1;
                transform.Rotate(Vector3.up,rot * Time.deltaTime);

                transform.position += transform.forward * _model.CurrentSpeed * Time.deltaTime;

                if ((t.position - transform.position).magnitude < 0.1f)
                {
                    Debug.Log("Target Reached");

                    _targetPlanet = null;
                    _model.State = ShipState.Idle;
                    TargetReached?.Invoke();
                }
            }
        }
    }
}
