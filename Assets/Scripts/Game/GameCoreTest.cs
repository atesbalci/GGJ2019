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
           _ship.SetValue(2f,10f,5f);
        }
    }
}
