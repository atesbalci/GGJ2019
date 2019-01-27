using System;
using System.Linq;
using Game.Models;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Behaviours
{
    public class PlanetBehaviour : MonoBehaviour
    {
        public Planet Planet { get; private set; }
        public const float LifeSupportCoeff = 25f; //experimental
        public const float FuelCoeff = 15f; //experimental

        public void Bind(Planet planet)
        {
            Planet = planet;
            SetParameters();
        }

        private void SetParameters()
        {
            SetPlanet();
            SetView();
        }

        private void SetPlanet()
        {
            Planet.LifeSupport = new FloatReactiveProperty(
                    Planet.PlanetType == PlanetType.LifeSupport ? Planet.Radius * LifeSupportCoeff : 0f);
            Planet.Fuel = new FloatReactiveProperty(
                    Planet.PlanetType == PlanetType.Fuel ? Planet.Radius * FuelCoeff : 0f);
        }

        public float Harvest(FloatReactiveProperty source, float flow)
        {
            var amount = Mathf.Clamp(source.Value, 0f, flow);
            if (amount > 0f)
            {
                source.Value -= amount * Time.deltaTime;
            }

            //todo: cache later
            var r = GetComponent<Renderer>();
            if (r != null)
            {

                var sourcePercentage = (Planet.LifeSupport.Value > 0 ? Planet.LifeSupport.Value : Planet.Fuel.Value) /
                                       (Planet.Radius * (Planet.LifeSupport.Value > 0 ? LifeSupportCoeff : FuelCoeff));
                r.material.color = Color.Lerp(Color.gray, Planet.PlanetColorMapping[Planet.PlanetType], sourcePercentage);
            }

            return amount;
        }

        private void SetView()
        {
            transform.localScale = Planet.Radius * Vector3.one;
            var r = GetComponent<Renderer>();
            if (r != null)
            {
                r.material.color = Planet.PlanetColorMapping[Planet.PlanetType];
            }
        }
            
        public class Pool : MonoMemoryPool<PlanetBehaviour> { }

    }
}