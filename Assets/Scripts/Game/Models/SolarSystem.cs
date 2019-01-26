using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Game.Models
{
    public class SolarSystem
    {
        public IList<Orbit> Orbits { get; }
        
        public SolarSystem()
        {
            Orbits = new List<Orbit>();
            for (int i = 0; i < 5; i++)
            {
                Orbits.Add(new Orbit(2f * (Random.value > 0.5f ? 1f : -1f), 5 * (i + 1)));
            }
        }
    }
}