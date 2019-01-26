using System;
using DG.Tweening;
using Game.Behaviours;
using UnityEngine;
using Zenject;

namespace Game.Models
{
    public class ShipBehaviour : MonoBehaviour
    {
        public Ship Model;
        public Transform Pivot;
        private Sequence sequence;
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

        private void Update()
        {
            if (_targetPlanet != null)
            {
                var t = _targetPlanet.transform;
                Model.CurrentSpeed = Mathf.Clamp(Model.CurrentSpeed + Model.Acceleration, 0f, Model.MaxMoveSpeed);

                var dir = (t.position - transform.position).normalized;
                var landingDistance = (t.position - transform.position).magnitude;

                if (landingDistance < _targetPlanet.Planet.Radius + 1f && Model.State != ShipState.Landing)
                {
                    Model.State = ShipState.Landing;
                }

                switch (Model.State)
                {
                    case ShipState.Idle:
                        //Life support consume
                        break;
                    case ShipState.Moving:
                        //Life support gain
                        //Fuel consume
                        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, dir, Model.RotationSpeed * Time.deltaTime));
                        transform.position += transform.forward * Model.CurrentSpeed * Time.deltaTime;
                        break;
                    case ShipState.Landing:
                        if (sequence == null)
                        {
                            sequence = DOTween.Sequence();
                            transform.SetParent(_targetPlanet.transform);
                            sequence.Append(transform.DORotateQuaternion(Quaternion.LookRotation(-dir), 2f).SetEase(Ease.Linear));
                            sequence.Join(transform.DOLocalMove(_targetPlanet.Planet.Radius/2f * _targetPlanet.transform.InverseTransformVector(-dir), 2f).SetEase(Ease.Linear));
                            sequence.OnComplete(() =>
                            {
                                _targetPlanet = null;
                                Model.State = ShipState.Idle;
                                TargetReached?.Invoke();
                                sequence.Kill();
                                sequence = null;
                            });
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
