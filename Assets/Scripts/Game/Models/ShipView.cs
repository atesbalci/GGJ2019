using System;
using UnityEngine;
using Zenject;

namespace Game.Models
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField]
        private TrailRenderer _trailRenderer;

        private ShipBehaviour _shipBehaviour;

        [Inject]
        private void Construct(ShipBehaviour shipBehaviour)
        {
            _shipBehaviour = shipBehaviour;
        }

        private void Awake()
        {
            _trailRenderer = GetComponentInChildren<TrailRenderer>();
            _shipBehaviour.Model.OnStateChanged += state => { _trailRenderer.emitting = state == ShipState.Moving; };
        }
    }
}