﻿using System;
using DG.Tweening;
using Game.Behaviours;
using UnityEngine;
using Zenject;

namespace Game.Models
{
    public class ShipBehaviour : MonoBehaviour
    {
        public Ship Ship;
        private Sequence sequence;

        public event Action TargetReachedEvent;
        public event Action RunOutOfFuelEvent;

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
            switch (Ship.State)
            {
                case ShipState.Idle:
                    HarvestLifeSupport(5f);
                    HarvestFuel(5f);
                    ConsumeLifeSupport(Ship.LifeSupportFlow.Value / 2f);
                    break;
                case ShipState.Moving:
                    ConsumeFuel(Ship.FuelFlow.Value);
                    ConsumeLifeSupport(Ship.LifeSupportFlow.Value);
                    Move();
                    break;
                case ShipState.Landing:
                    ConsumeFuel(Ship.FuelFlow.Value * 2f);
                    Land();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log("Fuel: " + Ship.Fuel);
            Debug.Log("Life Support: " + Ship.LifeSupport);

        }

        private void Move()
        {
            if (_targetPlanet != null)
            {
                var t = _targetPlanet.transform;
                Ship.CurrentSpeed = Mathf.Clamp(Ship.CurrentSpeed + Ship.Acceleration.Value, 0f, Ship.MaxMoveSpeed.Value);

                var dir = (t.position - transform.position).normalized;
                var landingDistance = (t.position - transform.position).magnitude;

                if (landingDistance < _targetPlanet.Planet.Radius + 1f && Ship.State != ShipState.Landing)
                {
                    Ship.State = ShipState.Landing;
                }

                transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, dir,
                    Ship.RotationSpeed.Value * Time.deltaTime));
                transform.position += transform.forward * Ship.CurrentSpeed * Time.deltaTime;
            }
        }

        private void Land()
        {
            if (sequence == null)
            {
                var t = _targetPlanet.transform;
                var dir = (t.position - transform.position).normalized;
                transform.SetParent(t.transform);

                sequence = DOTween.Sequence();
                sequence.Append(transform.DORotateQuaternion(Quaternion.LookRotation(-dir), 2f).SetEase(Ease.Linear));
                sequence.Join(transform
                    .DOLocalMove(_targetPlanet.Planet.Radius / 2f * _targetPlanet.transform.InverseTransformVector(-dir), 2f)
                    .SetEase(Ease.Linear));
                sequence.OnComplete(() =>
                {
                    //_targetPlanet = null;
                    Ship.State = ShipState.Idle;
                    TargetReachedEvent?.Invoke();
                    sequence.Kill();
                    sequence = null;
                });
            }
        }

        private void ConsumeFuel(float flow)
        {
            if (Ship.Fuel.Value < 0f)
            {
                return;
            }

            Ship.Fuel.Value = Mathf.Clamp(Ship.Fuel.Value - flow * Time.deltaTime, 0f, float.MaxValue);
            if (Ship.Fuel.Value <= 0f)
            {
                RunOutOfFuelEvent?.Invoke();
            }
        }

        private void ConsumeLifeSupport(float flow)
        {
            Ship.LifeSupport.Value = Mathf.Clamp(Ship.LifeSupport.Value - flow * Time.deltaTime, 0f, float.MaxValue);
        }

        private void HarvestLifeSupport(float flow)
        {
            if (_targetPlanet != null)
            {
                Ship.LifeSupport.Value += _targetPlanet.Harvest(_targetPlanet.Planet.LifeSupport ,flow) * Time.deltaTime;
            }
        }

        private void HarvestFuel(float flow)
        {
            if (_targetPlanet != null)
            {
                flow =  Mathf.Clamp(Ship.MaxFuel.Value - Ship.Fuel.Value, 0, flow);
                Ship.Fuel.Value += _targetPlanet.Harvest(_targetPlanet.Planet.Fuel,  flow) * Time.deltaTime;
            }
        }
    }
}
