using System.Reflection;
using Game.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class LevelView : MonoBehaviour
    {
        private LevelData _levelData;

        [SerializeField]
        private Button _nextLevelButton;



        private void Construct(LevelData levelData)
        {
            _levelData = levelData;
        }


    }
}