using System;
using DG.Tweening;
using Game.Behaviours;
using UnityEngine;
using Zenject;

namespace Game.Models
{
    public class ShipBehaviour : MonoBehaviour
    {
        public Ship Ship;
        public Transform Pivot;
        private Sequence sequence;
        public event Action TargetReached;

        [Inject]
        private void Construct(Ship ship)
        {
            Ship = ship;
        }
        
        //changed with planet later
        private PlanetBehaviour _targetPlanet;

        public void SetTarget(PlanetBehaviour target)
        {
            if (Ship.State == ShipState.Idle)
            {
                _targetPlanet = target;
                Ship.State = ShipState.Moving;
                transform.SetParent(null);
            }
        }

        private void Update()
        {
            if (_targetPlanet != null)
            {
                var t = _targetPlanet.transform;
                Ship.CurrentSpeed = Mathf.Clamp(Ship.CurrentSpeed + Ship.Acceleration, 0f, Ship.MaxMoveSpeed);

                var dir = (t.position - transform.position).normalized;
                var landingDistance = (t.position - transform.position).magnitude;

                if (landingDistance < _targetPlanet.Planet.Radius + 1f && Ship.State != ShipState.Landing)
                {
                    Ship.State = ShipState.Landing;
                }

                switch (Ship.State)
                {
                    case ShipState.Idle:
                        ConsumeLifeSupport(Ship.LifeSupportFlow/2f);
                        break;
                    case ShipState.Moving:
                        ConsumeFuel(Ship.FuelFlow);
                        ConsumeLifeSupport(Ship.LifeSupportFlow);

                        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, dir, Ship.RotationSpeed * Time.deltaTime));
                        transform.position += transform.forward * Ship.CurrentSpeed * Time.deltaTime;
                        break;
                    case ShipState.Landing:

                        ConsumeFuel(Ship.FuelFlow * 2f);

                        if (sequence == null)
                        {
                            sequence = DOTween.Sequence();
                            transform.SetParent(_targetPlanet.transform);
                            sequence.Append(transform.DORotateQuaternion(Quaternion.LookRotation(-dir), 2f).SetEase(Ease.Linear));
                            sequence.Join(transform.DOLocalMove(_targetPlanet.Planet.Radius/2f * _targetPlanet.transform.InverseTransformVector(-dir), 2f).SetEase(Ease.Linear));
                            sequence.OnComplete(() =>
                            {
                                _targetPlanet = null;
                                Ship.State = ShipState.Idle;
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

        private void ConsumeFuel(float flow)
        {
            Ship.Fuel -= flow * Time.deltaTime;
        }

        private void ConsumeLifeSupport(float flow)
        {
            Ship.LifeSupport -= flow* Time.deltaTime;
        }
    }
}
