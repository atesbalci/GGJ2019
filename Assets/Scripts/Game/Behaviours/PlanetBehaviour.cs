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
        public const float Fuel = 5f; //experimental

        public void Bind(Planet planet)
        {
            Planet = planet;
            SetPlanet();
            SetView();
        }

        private void SetPlanet()
        {
            Planet.Radius = Random.Range(0.5f, 3f); //test purpose
            Planet.PlanetType =
                Planet.PlanetColorMapping.Keys.ElementAt(Random.Range(0, Enum.GetNames(typeof(PlanetType)).Length - 1));
            Planet.LifeSupport = new FloatReactiveProperty(
                    Planet.PlanetType == PlanetType.LifeSupport ? Planet.Radius * LifeSupportCoeff : 0f);
            Planet.Fuel = new FloatReactiveProperty(
                    Planet.PlanetType == PlanetType.Fuel ? Planet.Radius * Fuel : 0f);
        }

        public float Harvest(FloatReactiveProperty source, float flow)
        {
            var amount = Mathf.Clamp(source.Value, 0f, flow);
            if (amount > 0f)
            {
                source.Value -= amount * Time.deltaTime;
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