using Game.Models;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameCoreTest : MonoBehaviour
    {
        private Ship _ship;

        [Inject]
        private void Construct(Ship ship)
        {
            _ship = ship;
        }

        private void Start()
        {
            _ship.SetShipParameters(2f, 5f, 5f, 100, 100, 5f, 2f);
            _ship.FillConsumables();
        }
    }
}
