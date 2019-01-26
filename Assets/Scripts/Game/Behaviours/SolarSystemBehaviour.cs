using System.Collections.Generic;
using Game.Models;
using UnityEngine;
using Zenject;

namespace Game.Behaviours
{
    public class SolarSystemBehaviour : MonoBehaviour
    {
        private SolarSystem _solarSystem;
        private ICollection<PlanetBehaviour> _planets; 

        [Inject]
        public void Initialize(SolarSystem solarSystem)
        {
            _solarSystem = solarSystem;
            _planets = new LinkedList<PlanetBehaviour>();
        }

        private void Update()
        {
            var center = transform.position;
            var time = Time.time;
            foreach (var planetBehaviour in _planets)
            {
                var orbit = planetBehaviour.Planet.Orbit;
                var angle = (time * orbit.Speed) / (2 * orbit.Radius);
                planetBehaviour.transform.position = new Vector3(center.x + Mathf.Cos(angle) * orbit.Radius, 0f,
                    center.z + Mathf.Sin(angle) * orbit.Radius);
            }
        }

        public void AddPlanet(PlanetBehaviour planetBehaviour)
        {
            _planets.Add(planetBehaviour);
        }
    }
}