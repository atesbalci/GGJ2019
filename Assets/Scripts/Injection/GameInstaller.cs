using Game.Behaviours;
using Game.Models;
using UnityEngine;
using Zenject;

namespace Injection
{
    public class GameInstaller : MonoInstaller
    {
        public GameObject PlanetPrefab;
        public ShipBehaviour ShipBehaviour;
        
        public override void InstallBindings()
        {
            Container.Bind<SolarSystem>().AsSingle();
            Container.BindInstance(FindObjectOfType<SolarSystemBehaviour>()).AsSingle();
            Container.BindFactory<PlanetBehaviour, PlanetBehaviour.Factory>().FromComponentInNewPrefab(PlanetPrefab);
            Container.Bind<Ship>().AsSingle();
            Container.BindInstance(ShipBehaviour).AsSingle();
        }
    }
}