using Game.Data;
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
        private InteractionData _interactionData;

        [Inject]
        private void Construct(ShipBehaviour shipBehaviour, InteractionData interactionData)
        {
            _shipBehaviour = shipBehaviour;
            _interactionData = interactionData;
            _camera = Camera.main;
        }

        private void Update()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            PlanetView planetView = null;
            if (Physics.Raycast(ray, out var hit))
            {
                planetView = hit.transform.gameObject.GetComponent<PlanetView>();
                if (planetView != null)
                {
                    planetView.IsUnderCursor = true;
                    if (Input.GetMouseButtonDown(0))
                    {
                        _shipBehaviour.SetTarget(planetView.PlanetBehaviour);
                    }
                }
            }

            _interactionData.CurrentlySelectedPlanet.Value = planetView;
        }
    }
}
