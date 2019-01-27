using System;
using System.Collections.Generic;
using System.Linq;
using Game.Behaviours;
using Game.Models;
using Helpers.Utility;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        private SolarSystemBehaviour _solarSystemBehaviour;
        private SolarSystem _solarSystem;
        private ShipBehaviour _shipBehaviour;
        private LevelData _levelData;

        [Inject]
        public void Initialize(
            SolarSystemBehaviour solarSystemBehaviour,
            SolarSystem solarSystem,
            ShipBehaviour shipBehaviour,
            LevelData levelData)
        {
            _solarSystemBehaviour = solarSystemBehaviour;
            _solarSystem = solarSystem;
            _shipBehaviour = shipBehaviour;
            _levelData = levelData;

            StartLevel();

            _shipBehaviour.RunOutOfFuelEvent += RunOutOfFuel;
            _shipBehaviour.TargetReachedEvent += HomePlanetReached;
        }

        private void StartLevel()
        {
            SetLevel();
            SetPlanets();
            SetShip();
        }

        private void SetLevel()
        {
            _levelData.RequiredLifeSupport = _levelData.Level.Value * 25 + 100f;
            _solarSystemBehaviour.ClearPlanets();
            _solarSystem.InitializeOrbits(_levelData.Level.Value + LevelData.InitialPlanetCount);
        }
        
        /// <summary>
        ///FirstPlanetHome
        ///MidPlanetsLifeSupport
        ///LaterPlanetsFuel 
        /// </summary>
        private void SetPlanets()
        {
            var planets = new List<Planet>();
            for (var i = 0; i < LevelData.InitialPlanetCount + _levelData.Level.Value - 1; i++)
            {
                planets.Add(new Planet(_solarSystem.Orbits.RandomElementRemove()));
            }

            var home = planets.First();
            home.PlanetType = PlanetType.Home;
            home.Radius = 2f;

            var fuelPlanetCount = Mathf.FloorToInt(planets.Count / 3f);

            for (var i = 1; i < planets.Count; i++)
            {
                var planet = planets[i];
                planet.PlanetType = i >= planets.Count - fuelPlanetCount ? PlanetType.Fuel : PlanetType.LifeSupport;
            }

            var lifeSupportCount = planets.Count - fuelPlanetCount;
            var remainingLifeSupport = _levelData.RequiredLifeSupport * 1.5f;
            for (var i = 1; i < lifeSupportCount; i++)
            {
                var p = planets[i];
                if (i == lifeSupportCount - 1)
                {
                    p.Radius = remainingLifeSupport / PlanetBehaviour.LifeSupportCoeff;
                }
                else
                {
                    var a = remainingLifeSupport / (PlanetBehaviour.LifeSupportCoeff * lifeSupportCount) + Random.value;
                    p.Radius = Random.Range(1f, a);
                    remainingLifeSupport -= p.Radius * PlanetBehaviour.LifeSupportCoeff;
                }
            }

            for (var i = lifeSupportCount; i < planets.Count; i++)
            {
                var p = planets[i];
                p.Radius = Random.Range(0.5f, 1f);
            }

            foreach (var planet in planets)
            {
                Debug.Log(planet.PlanetType);
                _solarSystemBehaviour.AddPlanet(planet);
            }
        }

        private void SetShip()
        {
            var ship = _shipBehaviour.Ship;
            if (ship != null)
            {
                var upgrades = _levelData.Upgrades;
                var moveSpeed = 4f + 0.5f * upgrades.MoveSpeed.Value;
                var maxFuel = 100f + 10f * upgrades.MaxFuel.Value;
                var fuelFlow = 5f - 0.2f * upgrades.FuelEfficiency.Value;
                var lifeSupportFlow = 1f - 0.075f * upgrades.LifeSupportEfficiency.Value;

                ship.SetShipParameters(moveSpeed, 5f, 5f, maxFuel, 100, fuelFlow, lifeSupportFlow); //Set according to user data
                ship.FillConsumables();
            }

            _shipBehaviour.Ship.State = ShipState.Idle;
            var planets = _solarSystem.Planets.OrderBy(p => p.Orbit.Radius).ToList();
            foreach (var planet in planets)
            {
                var planetBehaviour = _solarSystemBehaviour.Planets.FirstOrDefault(p => p.Planet == planet);
                if (planetBehaviour != null)
                {
                    _shipBehaviour.TargetPlanet = planetBehaviour;
                    var t = _shipBehaviour.transform;
                    t.SetParent(null);
                    t.localScale = Vector3.one * 0.75f;
                    t.SetParent(planetBehaviour.transform);
                    t.localPosition = new Vector3(0f,0f,0.5f);
                    t.rotation = Quaternion.LookRotation(Vector3.forward);
                }
            }
        }

        private void HomePlanetReached()
        {
            if (_shipBehaviour.Ship.LifeSupport.Value >= _levelData.RequiredLifeSupport)
            {
                ChangeLevel(_levelData.Level.Value++);
            }
        }

        //End Game UI / Next
        private void ChangeLevel(int level)
        {
            _levelData.Score += _shipBehaviour.Ship.Fuel.Value + _shipBehaviour.Ship.LifeSupport.Value;
            _levelData.Level.Value = level;
            StartLevel();
            _levelData.LevelChanged?.Invoke();
        }

        //End Game UI/ Restart game
        private void RunOutOfFuel()
        {
            _levelData.Reset();
            StartLevel();
            _levelData.LevelChanged?.Invoke();
        }
    }

    public class LevelData
    {
        public const int InitialPlanetCount = 5;

        public float RequiredLifeSupport;
        public float Score;
        public IntReactiveProperty Level;
        public ShipUpgrades Upgrades;
        public Action LevelChanged;

        public void Reset()
        {
            Score = 0f;
            Level.Value = 0;
            Upgrades = new ShipUpgrades();
        }
         
        public LevelData()
        {
            Level = new IntReactiveProperty(0);
            Upgrades = new ShipUpgrades();
        }

        public class ShipUpgrades
        {
            public IntReactiveProperty MoveSpeed;
            public IntReactiveProperty MaxFuel;
            public IntReactiveProperty FuelEfficiency;
            public IntReactiveProperty LifeSupportEfficiency;

            public ShipUpgrades()
            {
                MoveSpeed = new IntReactiveProperty(0);
                MaxFuel = new IntReactiveProperty(0);
                FuelEfficiency = new IntReactiveProperty(0);
                LifeSupportEfficiency = new IntReactiveProperty(0);
            }
        }
    }
}