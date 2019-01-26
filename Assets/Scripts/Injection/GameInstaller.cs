using Game.Behaviours;
using Game.Data;
using Game.Models;
using Game.Views;
using UnityEngine;
using Zenject;

namespace Injection
{
    public class GameInstaller : MonoInstaller
    {
        public Transform PlanetsParent;
        public Transform OrbitViewsParent;
        public GameObject PlanetPrefab;
        public GameObject OrbitViewPrefab;
        public ShipBehaviour ShipBehaviour;
        
        public override void InstallBindings()
        {
            Container.Bind<SolarSystem>().AsSingle();
            Container.BindInstance(FindObjectOfType<SolarSystemBehaviour>()).AsSingle();
            Container.BindMemoryPool<PlanetBehaviour, PlanetBehaviour.Pool>().WithInitialSize(10)
                .FromComponentInNewPrefab(PlanetPrefab).UnderTransform(PlanetsParent);
            Container.BindMemoryPool<OrbitView, OrbitView.Pool>().WithInitialSize(10)
                .FromComponentInNewPrefab(OrbitViewPrefab).UnderTransform(OrbitViewsParent);
            Container.Bind<Ship>().AsSingle();
            Container.BindInstance(ShipBehaviour).AsSingle();
            Container.Bind<InteractionData>().AsSingle();
        }
    }
}