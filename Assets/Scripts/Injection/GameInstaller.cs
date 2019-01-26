using Game.Models;
using UnityEngine;
using Zenject;

namespace Injection
{
    public class GameInstaller : MonoInstaller
    {
        public ShipBehaviour ShipBehaviour;
        
        public override void InstallBindings()
        {
            Container.Bind<SolarSystem>().AsSingle();
            Container.Bind<Ship>().AsSingle();
            Container.BindInstance(ShipBehaviour).AsSingle();
        }
    }
}