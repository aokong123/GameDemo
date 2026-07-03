using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.UI
{
    public class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;
    
        private Action<int> _onClick;
        private int _levelNumber;

        public void Setup(Action<int> onClick, int levelNumber, bool isInteractable)
        {
            _levelNumber = levelNumber;
            _buttonText.text = levelNumber.ToString();
            _onClick = onClick;
            _button.interactable = isInteractable;
            AddListeners();
        }

        private void AddListeners()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _onClick?.Invoke(_levelNumber);
        }

        public void RemoveListeners()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
