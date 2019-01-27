using Game.Models;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class ShipInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fuelText;
        [SerializeField] private TextMeshProUGUI _resourcesText;
        private Ship _ship;

        [Inject]
        public void Initialize(Ship ship)
        {
            _ship = ship;
            ship.Fuel.Subscribe(val => RefreshTexts()).AddTo(gameObject);
            ship.LifeSupport.Subscribe(val => RefreshTexts()).AddTo(gameObject);
        }

        private void RefreshTexts()
        {
            _fuelText.text = _ship.Fuel.Value.ToString("n0");
            _resourcesText.text = _ship.LifeSupport.Value.ToString("n0");
        }
    }
}