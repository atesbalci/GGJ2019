using Game.Behaviours;
using UnityEngine;

namespace Game.Views
{
    public class PlanetView : MonoBehaviour
    {
        public PlanetBehaviour PlanetBehaviour { get; private set; }

        [SerializeField] private Renderer _selection;
        private bool _isUnderCursor;

        private void Awake()
        {
            PlanetBehaviour = GetComponent<PlanetBehaviour>();
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