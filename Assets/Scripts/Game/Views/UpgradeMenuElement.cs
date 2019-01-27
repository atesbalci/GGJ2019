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

        public void Bind(IntReactiveProperty source, string desc, GameData gameData)
        {
            if (source != null)
            {
                _addButton.onClick.AddListener(() =>
                {
                    if (gameData.RemainingUpgrades.Value > 0)
                    {
                        source.Value++;
                        gameData.RemainingUpgrades.Value--;
                    }
                });

                _removeButton.onClick.AddListener(() => { source.Value--; });
                source.Subscribe(i => { _amountText.text = i.ToString(); });
                _descText.text = desc;
            }
        }
    }
}