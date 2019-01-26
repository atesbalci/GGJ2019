using System.Collections.Generic;
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
        public PlanetType PlanetType;

        public FloatReactiveProperty Fuel;
        public FloatReactiveProperty LifeSupport;

        public static Dictionary<PlanetType, Color> PlanetColorMapping = new Dictionary<PlanetType, Color>
        {
            {PlanetType.None, Color.gray },
            {PlanetType.LifeSupport, Color.green },
            {PlanetType.Fuel, Color.cyan },
            {PlanetType.Home, Color.yellow },
        };

        public Planet(Orbit orbit)
        {
            Orbit = orbit;
        }
    }
}