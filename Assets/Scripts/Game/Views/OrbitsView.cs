using System.Collections.Generic;
using Game.Behaviours;
using Game.Models;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class OrbitsView : MonoBehaviour
    {
        private OrbitView.Pool _orbitViewPool;
        private SolarSystem _solarSystem;
        private ICollection<OrbitView> _instantiatedOrbitViews;

        [Inject]
        public void Initialize(OrbitView.Pool orbitViewPool, SolarSystem solarSystem)
        {
            _orbitViewPool = orbitViewPool;
            _solarSystem = solarSystem;
            _instantiatedOrbitViews = new LinkedList<OrbitView>();
        }

        private void Start()
        {
            RefreshOrbitViews();
        }

        private void RefreshOrbitViews()
        {
            foreach (var orbitView in _instantiatedOrbitViews)
            {
                _orbitViewPool.Despawn(orbitView);
            }
            _instantiatedOrbitViews.Clear();
            foreach (var planet in _solarSystem.Planets)
            {
                var orbitView = _orbitViewPool.Spawn();
                orbitView.Bind(planet.Orbit);
                _instantiatedOrbitViews.Add(orbitView);
            }
        }
    }
}