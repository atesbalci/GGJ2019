using System.Reflection;
using Game.Controllers;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Views
{
    public class LevelView : MonoBehaviour
    {
        private GameData _gameData;
        private GameController _gameController;

        [SerializeField]
        private Button _nextLevelButton;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Transform _upgradeParent;
        [SerializeField]
        private Transform _gameOverParent;
        [SerializeField]
        private GameObject _buttonPrefab;
        [SerializeField]
        private TMP_Text _scoreText;
        [SerializeField]
        private TMP_Text _RemainingUpgrades;

        [Inject]
        private void Construct(GameData gameData, GameController gameController)
        {
            _gameData = gameData;
            _gameController = gameController;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
            _gameData.GameStateChanged += state =>
            {
                var success = _gameData.GameState == GameState.Successful;
                _nextLevelButton.gameObject.SetActive(success);
                _restartButton.gameObject.SetActive(!success);
                _upgradeParent.gameObject.SetActive(success);
                _gameOverParent.gameObject.SetActive(!success);
                _scoreText.text = "Score: " + _gameData.Score.ToString("N0");
                gameObject.SetActive(true);
            };

            _nextLevelButton.onClick.AddListener(() =>
            {
                _gameController.LoadLevel(++_gameData.Level.Value);
                gameObject.SetActive(false);
            });

            _restartButton.onClick.AddListener(() =>
            {
                _gameController.Restart();
                gameObject.SetActive(false);
            });

            _gameData.RemainingUpgrades.Subscribe(i => { _RemainingUpgrades.text = "Remaining Upgrades:  " + i; });

            var u = _gameData.Upgrades;
            AddButton(u.MaxFuel,"Increase Max Fuel Capacity");
            AddButton(u.MoveSpeed,"Increase Movement Speed");
            AddButton(u.FuelEfficiency,"Consume Less Fuel");
            AddButton(u.LifeSupportEfficiency,"Consume Less LifeSupport");
        }

        private void AddButton(IntReactiveProperty prop, string desc)
        {
            var button = Instantiate(_buttonPrefab, _upgradeParent);
            if (button != null)
            {
                var element = button.GetComponent<UpgradeMenuElement>();
                if (element != null)
                {
                    element.Bind(prop, desc,_gameData);
                }
            }
        }
    }
}