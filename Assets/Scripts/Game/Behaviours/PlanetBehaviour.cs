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
            SetView();
        }

        //Maybe color set
        private void SetView()
        {
            transform.localScale = Planet.Radius * Vector3.one;
        }

        
        public class Factory : PlaceholderFactory<PlanetBehaviour> { }
    }
}