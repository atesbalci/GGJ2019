using Game.Models;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Behaviours
{
    public class PlanetBehaviour : MonoBehaviour
    {
        public Planet Planet { get; private set; }
        public const float LifeSupportCoeff = 25f; //experimental

        public void Bind(Planet planet)
        {
            Planet = planet;
            SetPlanet();
            SetView();
        }

        private void SetPlanet()
        {
            Planet.Radius = Random.Range(0.5f, 3f); //test purpose
            Planet.LifeSupport = new FloatReactiveProperty(Planet.Radius * LifeSupportCoeff);


        }

        public float Harvest(float flow)
        {
            var amount = Mathf.Clamp(Planet.LifeSupport.Value, 0f, flow);
            if (amount > 0f)
            {
                Planet.LifeSupport.Value -= amount * Time.deltaTime;
            }

            return amount;
        }

        private void SetView()
        {
            transform.localScale = Planet.Radius * Vector3.one;
        }
            
        public class Pool : MonoMemoryPool<PlanetBehaviour> { }

    }
}