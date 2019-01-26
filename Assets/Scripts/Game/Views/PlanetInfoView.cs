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
        private const float YPixelOffset = 200f;
        
        public PlanetView CurrentPlanet { get; private set; }

        private TextMeshProUGUI _content;
        private ICollection<IDisposable> _subscriptions;
        private Camera _camera;
        private RectTransform _canvas;

        private RectTransform RectTransform => (RectTransform) transform;

        [Inject]
        public void Initialize(InteractionData interactionData)
        {
            _subscriptions = new LinkedList<IDisposable>();
            _content = GetComponentInChildren<TextMeshProUGUI>();
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
            var builder = new StringBuilder();
            builder.AppendLine("Resources: " + planet.LifeSupport.Value.ToString("n0"));
            _content.text = builder.ToString();
        }

        private void RefreshPosition()
        {
            if (CurrentPlanet)
            {
                Vector2 viewportPos = _camera.WorldToViewportPoint(CurrentPlanet.transform.position);
                var canvasSize = _canvas.sizeDelta;
                RectTransform.anchoredPosition =
                    new Vector2(viewportPos.x * canvasSize.x, viewportPos.y * canvasSize.y + YPixelOffset);
            }
        }
    }
}