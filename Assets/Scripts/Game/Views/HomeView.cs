using System.Linq;
using Game.Behaviours;
using Game.Controllers;
using Game.Models;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class HomeView : MonoBehaviour
    {
        private static readonly Vector2 PixelOffset = new Vector2(5f, 5f);
        
        private GameData _gameData;
        private SolarSystemBehaviour _solarSystemBehaviour;
        private Camera _camera;
        private PlanetBehaviour _planet;
        private RectTransform _canvas;

        private RectTransform RectTransform => (RectTransform) transform;

        [Inject]
        public void Initialize(GameData gameData, SolarSystemBehaviour solarSystemBehaviour)
        {
            _gameData = gameData;
            _solarSystemBehaviour = solarSystemBehaviour;
            _camera = Camera.main;
            _canvas = (RectTransform) GetComponentInParent<Canvas>().transform;
            _gameData.LevelChanged += LevelChanged;
        }

        private void Update()
        {
            RefreshPosition();
        }

        private void OnDestroy()
        {
            if (_gameData == null) return;
            _gameData.LevelChanged -= LevelChanged;
        }

        private void LevelChanged()
        {
            _planet = _solarSystemBehaviour.Planets.FirstOrDefault(planet => planet.Planet.PlanetType == PlanetType.Home);
            gameObject.SetActive(_planet);
            RefreshPosition();
        }

        private void RefreshPosition()
        {
            if (!_planet) return;
            var viewportPos = (Vector2) _camera.WorldToViewportPoint(_planet.transform.position) +
                              Mathf.Abs(_camera.WorldToViewportPoint(Vector3.zero).x - _camera
                                            .WorldToViewportPoint(
                                                _planet.Planet.Radius * Vector3.right).x) *
                              new Vector2(-1f, 1f);
            var canvasSize = _canvas.sizeDelta;
            var anchPos = new Vector2(viewportPos.x * canvasSize.x, viewportPos.y * canvasSize.y);
            RectTransform.anchoredPosition = anchPos;
        }
    }
}