using UnityEngine;
using Zenject;
namespace Game.Models.Input
{
    public class GameInteractor : MonoBehaviour
    {
        private ShipBehaviour _shipBehaviour;

        [Inject]
        private void Construct(ShipBehaviour shipBehaviour)
        {
            _shipBehaviour = shipBehaviour;
        }

        private void Update()
        {
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out var hit, 100))
                {
                    var planet = hit.transform.gameObject.GetComponent<PlanetBehaviour>();
                    if (planet != null)
                    {
                        _shipBehaviour.SetTarget(planet);
                    }
                }
            }
        }
    }
}
