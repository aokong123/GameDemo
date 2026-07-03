using System;
using CoreDomain.GameDomain.Scripts.Services.Levels.ScriptableObjects;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.UI
{
    public class LevelsButtonsView : MonoBehaviour
    {
        [SerializeField] private LevelButtonView _levelButtonPrefab;
        [SerializeField] private Transform _buttonsContainer;
        
        private LevelButtonView[] _levelButtonViews;
        private Action<int> _onLevelButtonClicked;

        public void Setup(LevelData[] levels, int maxLevelNumberReached, Action<int> onLevelButtonClicked)
        {
            _onLevelButtonClicked = onLevelButtonClicked;
            _levelButtonViews = new LevelButtonView[levels.Length];
            CreateLevelButtons(levels, maxLevelNumberReached);
        }

        private void CreateLevelButtons(LevelData[] levels, int maxLevelNumberReached)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                var levelButton = Instantiate(_levelButtonPrefab, _buttonsContainer);
                var isButtonInteractable = i < maxLevelNumberReached;
                levelButton.Setup(_onLevelButtonClicked, i + 1, isButtonInteractable);
                _levelButtonViews[i] = levelButton;
            }
        }

        public void Dispose()
        {
            foreach (var _levelButtonView in _levelButtonViews)
            {
                _levelButtonView.RemoveListeners();
                Destroy(_levelButtonView);
            }
        }
    }
}
