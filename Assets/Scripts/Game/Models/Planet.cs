using UnityEngine;

namespace Game.Models
{
    public class Planet
    {
        public Orbit Orbit { get; }
        public float Radius;

        public Planet(Orbit orbit)
        {
            Orbit = orbit;
            Radius = Random.Range(0.5f,3f); //test purpose
        }
    }
}