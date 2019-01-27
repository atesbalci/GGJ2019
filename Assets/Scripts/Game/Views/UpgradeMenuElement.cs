using Game.Controllers;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Views
{
    public class UpgradeMenuElement : MonoBehaviour
    {
        [SerializeField]
        private Button _addButton;
        [SerializeField]
        private Button _removeButton;
        [SerializeField]
        private TMP_Text _amountText;
        [SerializeField]
        private TMP_Text _descText;

        private IntReactiveProperty _source;
        private GameData _gameData;

        [Inject]
        private void Construct(GameData gameData)
        {
            _gameData = gameData;
        }

        public void Bind(IntReactiveProperty source, string desc)
        {
            if (source != null)
            {
                _addButton.onClick.AddListener(() =>
                {
                    source.Value++;
                    _gameData.RemainingUpgrades.Value--;
                });

                _removeButton.onClick.AddListener(() => { source.Value--; });
                source.Subscribe(i => { _amountText.text = i.ToString(); });
                _descText.text = desc;
            }
        }
    }
}