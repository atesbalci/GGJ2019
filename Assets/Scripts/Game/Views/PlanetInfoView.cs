using System;
using System.Collections.Generic;
using System.Text;
using Game.Data;
using Helpers.Utility;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Views
{
    public class PlanetInfoView : MonoBehaviour
    {
        private static readonly Vector2 PixelOffset = new Vector2(5f, 5f);
        
        public PlanetView CurrentPlanet { get; private set; }

        [SerializeField] private TextMeshProUGUI _fuelText;
        [SerializeField] private TextMeshProUGUI _resourcesText;
        private ICollection<IDisposable> _subscriptions;
        private Camera _camera;
        private RectTransform _canvas;

        private RectTransform RectTransform => (RectTransform) transform;

        [Inject]
        public void Initialize(InteractionData interactionData)
        {
            _subscriptions = new LinkedList<IDisposable>();
            _camera = Camera.main;
            _canvas = (RectTransform) GetComponentInParent<Canvas>().transform;
            interactionData.CurrentlySelectedPlanet.Subscribe(Show).AddTo(gameObject);
            Hide();
        }

        private void Update()
        {
            RefreshPosition();
        }

        private void Show(PlanetView planet)
        {
            CurrentPlanet = planet;
            if (CurrentPlanet == null)
            {
                Hide();
                return;
            }
            _subscriptions.DisposeAndClear();
            _subscriptions.Add(CurrentPlanet.PlanetBehaviour.Planet.LifeSupport.Subscribe(f => RefreshContent()));
            _subscriptions.Add(CurrentPlanet.PlanetBehaviour.Planet.Fuel.Subscribe(f => RefreshContent()));
            RefreshContent();
            RefreshPosition();
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            _subscriptions.DisposeAndClear();
            gameObject.SetActive(false);
        }

        private void RefreshContent()
        {
            var planet = CurrentPlanet.PlanetBehaviour.Planet;
            _resourcesText.text = planet.LifeSupport.Value.ToString("n0");
            _fuelText.text = planet.Fuel.Value.ToString("n0");
        }

        private void RefreshPosition()
        {
            if (CurrentPlanet)
            {
                var viewportPos = (Vector2) _camera.WorldToViewportPoint(CurrentPlanet.transform.position) +
                                      Mathf.Abs(_camera.WorldToViewportPoint(Vector3.zero).x - _camera
                                           .WorldToViewportPoint(
                                               CurrentPlanet.PlanetBehaviour.Planet.Radius * Vector3.right).x) *
                                      Vector2.one;
                var canvasSize = _canvas.sizeDelta;
                var anchPos = new Vector2(viewportPos.x * canvasSize.x, viewportPos.y * canvasSize.y);
                var size = RectTransform.sizeDelta;
                anchPos.x = Mathf.Clamp(anchPos.x, 0f, canvasSize.x - size.x);
                anchPos.y = Mathf.Clamp(anchPos.y, 0f, canvasSize.y - size.y);
                RectTransform.anchoredPosition = anchPos;
            }
        }
    }
}