using Game.Models;
using Game.Views;
using UnityEngine;
using Zenject;

namespace Game.Controllers
{
    public class GameInteractor : MonoBehaviour
    {
        private ShipBehaviour _shipBehaviour;
        private Camera _camera;
        private PlanetView _prevHighlitPlanet;

        [Inject]
        private void Construct(ShipBehaviour shipBehaviour)
        {
            _shipBehaviour = shipBehaviour;
            _camera = Camera.main;
        }

        private void Update()
        {
            var ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            PlanetView planet = null;
            if (Physics.Raycast(ray, out var hit))
            {
                planet = hit.transform.gameObject.GetComponent<PlanetView>();
                if (planet != null)
                {
                    planet.IsUnderCursor = true;
                    if (UnityEngine.Input.GetMouseButtonDown(0))
                    {
                        _shipBehaviour.SetTarget(planet.PlanetBehaviour);
                    }
                }
            }

            if (planet != _prevHighlitPlanet && _prevHighlitPlanet)
            {
                _prevHighlitPlanet.IsUnderCursor = false;
            }
            _prevHighlitPlanet = planet;
        }
    }
}
