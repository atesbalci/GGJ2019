using System.Linq;
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
        private RaycastHit[] _hits;

        [Inject]
        private void Construct(ShipBehaviour shipBehaviour, InteractionData interactionData)
        {
            _shipBehaviour = shipBehaviour;
            _interactionData = interactionData;
            _camera = Camera.main;
            _hits = new RaycastHit[10];
        }

        private void Update()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            PlanetView planetView = null;
            var hits = Physics.RaycastAll(ray);
            var closest = hits.OrderBy(hit =>
            {
                var mousePoint = hit.point;
                mousePoint.y = 0f;
                return Vector3.Distance(hit.transform.position, mousePoint);
            }).FirstOrDefault();
            if (closest.transform)
            {
                planetView = closest.transform.GetComponent<PlanetView>();
            }
            if (planetView != null)
            {
                planetView.IsUnderCursor = true;
                if (Input.GetMouseButtonDown(0))
                {
                    _shipBehaviour.SetTarget(planetView.PlanetBehaviour);
                }
            }

            _interactionData.CurrentlySelectedPlanet.Value = planetView;
        }
    }
}
