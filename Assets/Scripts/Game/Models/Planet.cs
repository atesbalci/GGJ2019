using UnityEngine;

namespace Game.Models
{
    public class Planet
    {
        public Orbit Orbit { get; }
        public float Radius;
        public float LifeSupport;

        public Planet(Orbit orbit)
        {
            Orbit = orbit;
        }
    }
}