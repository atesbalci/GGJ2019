using Game.Models;
using UnityEngine;
using Zenject;

namespace Game.Behaviours
{
    public class PlanetBehaviour : MonoBehaviour
    {
        public Planet Planet { get; private set; }

        public void Bind(Planet planet)
        {
            Planet = planet;
        }
        
        public class Factory : PlaceholderFactory<PlanetBehaviour> { }
    }
}