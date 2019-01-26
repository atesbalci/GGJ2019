using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Game.Models
{
    public class SolarSystem
    {
        public IList<Orbit> Orbits { get; }
        public ICollection<Planet> Planets { get; }
        
        public SolarSystem()
        {
            Orbits = new List<Orbit>();
            Planets = new LinkedList<Planet>();
            InitializeOrbits(5);
        }

        public void InitializeOrbits(int count)
        {
            Orbits.Clear();
            for (int i = 0; i < count; i++)
            {
                Orbits.Add(new Orbit(2f * (Random.value > 0.5f ? 1f : -1f), 5 * (i + 1)));
            }
        }
    }
}