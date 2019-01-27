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
    public enum GameState
    {
        Suspended,
        Running,
        Successful,
        Failed,
    }

    public class GameController : MonoBehaviour
    {
        private SolarSystemBehaviour _solarSystemBehaviour;
        private SolarSystem _solarSystem;
        private ShipBehaviour _shipBehaviour;
        private GameData _gameData;

        [Inject]
        public void Initialize(
            SolarSystemBehaviour solarSystemBehaviour,
            SolarSystem solarSystem,
            ShipBehaviour shipBehaviour,
            GameData gameData)
        {
            _solarSystemBehaviour = solarSystemBehaviour;
            _solarSystem = solarSystem;
            _shipBehaviour = shipBehaviour;
            _gameData = gameData;

            StartLevel();

            _shipBehaviour.RunOutOfFuelEvent += () =>
            {
                _gameData.Score += _shipBehaviour.Ship.LifeSupport.Value;
                _gameData.GameState = GameState.Failed;
            };

            _shipBehaviour.TargetReachedEvent += () =>
            {
                if (_shipBehaviour.Ship.LifeSupport.Value >= _gameData.RequiredLifeSupport)
                {
                    var newScore = _shipBehaviour.Ship.Fuel.Value + _shipBehaviour.Ship.LifeSupport.Value;
                    _gameData.RemainingUpgrades.Value += Mathf.FloorToInt(newScore);
                    _gameData.Score += newScore;
                    _gameData.GameState = GameState.Successful;
                }
            };
        }

        private void StartLevel()
        {
            SetLevel();
            SetPlanets();
            SetShip();

            _gameData.GameState = GameState.Running;
        }

        private void SetLevel()
        {
            _gameData.RequiredLifeSupport = _gameData.Level.Value * 25 + 100f;
            _solarSystemBehaviour.ClearPlanets();
            _solarSystem.InitializeOrbits(_gameData.Level.Value + GameData.InitialPlanetCount);
        }
        
        /// <summary>
        ///FirstPlanetHome
        ///MidPlanetsLifeSupport
        ///LaterPlanetsFuel 
        /// </summary>
        private void SetPlanets()
        {
            var planets = new List<Planet>();
            for (var i = 0; i < GameData.InitialPlanetCount + _gameData.Level.Value - 1; i++)
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
            var remainingLifeSupport = _gameData.RequiredLifeSupport * 1.5f;
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
                var upgrades = _gameData.Upgrades;
                var moveSpeed = 5f + 0.5f * upgrades.MoveSpeed.Value;
                var maxFuel = 100f + 10f * upgrades.MaxFuel.Value;
                var fuelFlow = 3f - 0.2f * upgrades.FuelEfficiency.Value;
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

        public void Restart()
        {
            _gameData.Reset();
            StartLevel();
            _gameData.LevelChanged?.Invoke();
        }

        public void LoadLevel(int level)
        {
            _gameData.Level.Value = level;
            StartLevel();
            _gameData.LevelChanged?.Invoke();
        }
    }

    public class GameData
    {
        private GameState _gameState;

        public GameState GameState
        {
            get => _gameState;
            set
            {
                if (value != _gameState)
                {
                    _gameState = value;
                    GameStateChanged?.Invoke(_gameState);
                }
            }
        }

        public const int InitialPlanetCount = 5;
        public float RequiredLifeSupport;
        public float Score;
        public IntReactiveProperty RemainingUpgrades;
        public IntReactiveProperty Level;
        public ShipUpgrades Upgrades;

        public Action LevelChanged;
        public Action<GameState> GameStateChanged;

        public void Reset()
        {
            Score = 0f;
            Level.Value = 0;
            Upgrades = new ShipUpgrades();
        }
         
        public GameData()
        {
            Level = new IntReactiveProperty();
            RemainingUpgrades = new IntReactiveProperty();
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
                MoveSpeed = new IntReactiveProperty();
                MaxFuel = new IntReactiveProperty();
                FuelEfficiency = new IntReactiveProperty();
                LifeSupportEfficiency = new IntReactiveProperty();
            }
        }
    }
}