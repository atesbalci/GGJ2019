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

        [Inject]
        public void Initialize(
            SolarSystemBehaviour solarSystemBehaviour,
            SolarSystem solarSystem)
        {
            _solarSystemBehaviour = solarSystemBehaviour;
            _solarSystem = solarSystem;
            StartGame();
        }

        private void StartGame()
        {
            _solarSystem.InitializeOrbits();
            _solarSystemBehaviour.ClearPlanets();
            for (int i = 0; i < 3; i++)
            {
                var planet = new Planet(_solarSystem.Orbits.RandomElementRemove());
                _solarSystemBehaviour.AddPlanet(planet);
            }
        }
    }
}