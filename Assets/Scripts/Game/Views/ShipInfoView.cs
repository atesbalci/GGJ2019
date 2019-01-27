using Game.Controllers;
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
        private LevelData _levelData;

        [Inject]
        public void Initialize(Ship ship, LevelData levelData)
        {
            _ship = ship;
            _levelData = levelData;
            ship.Fuel.Subscribe(val => RefreshTexts()).AddTo(gameObject);
            ship.LifeSupport.Subscribe(val => RefreshTexts()).AddTo(gameObject);
        }

        private void RefreshTexts()
        {
            _fuelText.text = _ship.Fuel.Value.ToString("n0");
            _resourcesText.text = _ship.LifeSupport.Value.ToString("n0") + "/" +
                                  _levelData.RequiredLifeSupport.ToString("n0");
        }
    }
}