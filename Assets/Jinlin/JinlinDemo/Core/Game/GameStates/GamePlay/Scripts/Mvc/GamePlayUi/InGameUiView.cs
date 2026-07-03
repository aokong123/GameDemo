using System;
using System.Threading;
using CoreDomain.Scripts.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi
{
    public class InGameUiView : MonoBehaviour
    {
        [SerializeField] private CountableTextView _scoreCountable;
        [SerializeField] private Button _exitButton;
        
        private Action _onExitButtonClicked;

        public void InitEntryPoint(Action onExitButtonClicked)
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _onExitButtonClicked = onExitButtonClicked;
        }

        private void OnExitButtonClicked()
        {
            _onExitButtonClicked?.Invoke();
        }
        
        public void SetCurrentScoreText(int score, CancellationTokenSource cancellationTokenSource, bool withAnimation = true)
        {
            if (withAnimation)
            {
                _scoreCountable.CountToNumber(score, cancellationTokenSource);
            }
            else
            {
                _scoreCountable.SetNumber(score);
            }
        }

        public void InitExitPoint()
        {
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);   
        }

        public void SetScoreGoal(int scoreGoal)
        {
            _scoreCountable.SetGoalNumber(scoreGoal);
        }
    }
}
