using System.Collections.Generic;
using Game.Behaviours;
using Game.Controllers;
using Game.Models;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class OrbitsView : MonoBehaviour
    {
        private OrbitView.Pool _orbitViewPool;
        private SolarSystem _solarSystem;
        private LevelData _levelData;
        private ICollection<OrbitView> _instantiatedOrbitViews;

        [Inject]
        public void Initialize(OrbitView.Pool orbitViewPool, SolarSystem solarSystem, LevelData levelData)
        {
            _orbitViewPool = orbitViewPool;
            _solarSystem = solarSystem;
            _levelData = levelData;
            _instantiatedOrbitViews = new LinkedList<OrbitView>();
        }

        private void Start()
        {
            RefreshOrbitViews();
            _levelData.LevelChanged += RefreshOrbitViews;
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

        private void OnDestroy()
        {
            _levelData.LevelChanged -= RefreshOrbitViews;
        }
    }
}