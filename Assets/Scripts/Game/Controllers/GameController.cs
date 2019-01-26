using Game.Behaviours;
using Game.Models;
using Helpers.Utility;
using UnityEngine;
using Zenject;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        private SolarSystemBehaviour _solarSystemBehaviour;
        private SolarSystem _solarSystem;
        private PlanetBehaviour.Factory _planetFactory;

        [Inject]
        public void Initialize(
            SolarSystemBehaviour solarSystemBehaviour,
            SolarSystem solarSystem,
            PlanetBehaviour.Factory planetFactory)
        {
            _solarSystemBehaviour = solarSystemBehaviour;
            _solarSystem = solarSystem;
            _planetFactory = planetFactory;
            for (int i = 0; i < 3; i++)
            {
                var planetBehaviour = _planetFactory.Create();
                planetBehaviour.Bind(new Planet(_solarSystem.Orbits.RandomElementRemove()));
                _solarSystemBehaviour.AddPlanet(planetBehaviour);
            }
        }
    }
}