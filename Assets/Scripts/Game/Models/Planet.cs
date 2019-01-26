using UniRx;
using UnityEngine;

namespace Game.Models
{
    public enum PlanetType
    {
        None,
        LifeSupport,
        Fuel,
        Home,
    }

    public class Planet
    {
        public Orbit Orbit { get; }
        public float Radius;
        public FloatReactiveProperty LifeSupport;

        public Planet(Orbit orbit)
        {
            Orbit = orbit;
        }
    }
}