using Game.Controllers;
using Game.Models;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;
using Game.Data;

namespace Game.Views
{
    public class ShipInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fuelText;
        [SerializeField] private TextMeshProUGUI _resourcesText;
        private Ship _ship;
        private GameData _gameData;

        [Inject]
        public void Initialize(Ship ship, GameData gameData)
        {
            _ship = ship;
            _gameData = gameData;
            ship.Fuel.Subscribe(val => RefreshTexts()).AddTo(gameObject);
            ship.LifeSupport.Subscribe(val => RefreshTexts()).AddTo(gameObject);
        }

        private void RefreshTexts()
        {
            _fuelText.text = _ship.Fuel.Value.ToString("n0");
            _resourcesText.text = _ship.LifeSupport.Value.ToString("n0") + "/" +
                                  _gameData.RequiredLifeSupport.ToString("n0");
        }
    }
}