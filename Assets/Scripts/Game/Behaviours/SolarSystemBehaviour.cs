using System.Collections.Generic;
using Game.Models;
using UnityEngine;
using Zenject;

namespace Game.Behaviours
{
    public class SolarSystemBehaviour : MonoBehaviour
    {
        private SolarSystem _solarSystem;
        private PlanetBehaviour.Pool _planetPool;
        public ICollection<PlanetBehaviour> Planets { get; private set; }

        [Inject]
        public void Initialize(SolarSystem solarSystem, PlanetBehaviour.Pool planetPool)
        {
            _solarSystem = solarSystem;
            _planetPool = planetPool;
            Planets = new LinkedList<PlanetBehaviour>();
        }

        private void Update()
        {
            var center = transform.position;
            var time = Time.time;
            foreach (var planetBehaviour in Planets)
            {
                var orbit = planetBehaviour.Planet.Orbit;
                var angle = (time * orbit.Speed) / (2 * orbit.Radius);
                planetBehaviour.transform.position = new Vector3(center.x + Mathf.Cos(angle) * orbit.Radius, 0f,
                    center.z + Mathf.Sin(angle) * orbit.Radius);
            }
        }

        public void AddPlanet(Planet planet)
        {
            var planetBehaviour = _planetPool.Spawn();
            planetBehaviour.Bind(planet);
            Planets.Add(planetBehaviour);
            _solarSystem.Planets.Add(planet);
        }

        public void ClearPlanets()
        {
            foreach (var planet in Planets)
            {
                _planetPool.Despawn(planet);
            }
            _solarSystem.Planets.Clear();
            Planets.Clear();
        }
    }
}