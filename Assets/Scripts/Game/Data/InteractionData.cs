using Game.Models;
using Game.Views;
using UniRx;

namespace Game.Data
{
    public class InteractionData
    {
        public ReactiveProperty<PlanetView> CurrentlySelectedPlanet { get; }

        public InteractionData()
        {
            CurrentlySelectedPlanet = new ReactiveProperty<PlanetView>(null);
        }
    }
}