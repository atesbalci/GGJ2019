using Game.Behaviours;
using Game.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetView : MonoBehaviour
    {
        public PlanetBehaviour PlanetBehaviour { get; private set; }

        [SerializeField] private Renderer _selection;
        private bool _isUnderCursor;

        [Inject]
        public void Initialize(InteractionData interactionData)
        {
            PlanetBehaviour = GetComponent<PlanetBehaviour>();
            interactionData.CurrentlySelectedPlanet.Subscribe(planet =>
            {
                IsUnderCursor = planet == this && gameObject.activeSelf;
            }).AddTo(gameObject);
        }

        public bool IsUnderCursor
        {
            get => _isUnderCursor;
            set
            {
                if(_isUnderCursor == value) return;
                _isUnderCursor = value;
                _selection.gameObject.SetActive(value);
            }
        }
    }
}