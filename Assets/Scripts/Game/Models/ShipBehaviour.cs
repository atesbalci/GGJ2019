using System;
using Game.Behaviours;
using UnityEngine;
using Zenject;

namespace Game.Models
{
    public class ShipBehaviour : MonoBehaviour
    {
        public Ship Model;

        public event Action TargetReached;

        [Inject]
        private void Construct(Ship model)
        {
            Model = model;
        }
        
        //changed with planet later
        private PlanetBehaviour _targetPlanet;

        public void SetTarget(PlanetBehaviour target)
        {
            if (Model.State == ShipState.Idle)
            {
                _targetPlanet = target;
                Model.State = ShipState.Moving;
                transform.SetParent(null);
            }
        }

        private void Awake()
        {
            Model.State = ShipState.Idle;  
        }

        private void Update()
        {
            if (_targetPlanet != null)
            {
                var t = _targetPlanet.transform;
                Model.CurrentSpeed = Mathf.Clamp(Model.CurrentSpeed + Model.Acceleration, 0f, Model.MaxMoveSpeed);

                var dir = (t.position - transform.position).normalized;

                #region Experimental Rotation

                //var angleBetween = Vector3.Angle(transform.forward, dir);
                //var rot =  angleBetween >= Model.RotationSpeed ? Model.RotationSpeed : angleBetween;
                //rot *= Vector3.Dot(dir, transform.right)>0 ? 1 : -1;
                //transform.Rotate(Vector3.up,rot * Time.deltaTime);

                #endregion

                transform.rotation =  Quaternion.LookRotation(Vector3.Lerp(transform.forward, dir, Model.RotationSpeed * Time.deltaTime));
                transform.position += transform.forward * Model.CurrentSpeed * Time.deltaTime;

                //check is in range of the planet
                if ((t.position - transform.position).magnitude < 0.25f)
                {
                    Debug.Log("Target Reached");

                    transform.SetParent(_targetPlanet.transform);
                    _targetPlanet = null;
                    Model.State = ShipState.Idle;
                    TargetReached?.Invoke();
                }
            }
        }
    }
}
