using Game.Models;
using Zenject;

namespace Injection
{
    public class GameInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<SolarSystem>().AsSingle();
        }
    }
}