using System;
using System.Threading;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi
{
    public class GamePlayUiView : MonoBehaviour
    {
        [SerializeField] private GameOverUiView _gameOverUiView;
        [SerializeField] private BeforeGameUiView _beforeGameUiView;
        [SerializeField] private InGameUiView _inGameUiView;
        [SerializeField] private WinUiView _winUiView;
        [SerializeField] private Canvas _screenCanvas;
        
        public void InitEntryPoint(Camera uiCamera, Action onExitButtonClicked)
        {
            _screenCanvas.worldCamera = uiCamera;
            _inGameUiView.InitEntryPoint(onExitButtonClicked);
            _beforeGameUiView.InitEntryPoint();
            _gameOverUiView.InitEntryPoint();
            _winUiView.InitEntryPoint();
        }
        
        public void ShowGameOverPanel(int score, int scoreGoal, bool shouldShowScore, CancellationTokenSource cancellationTokenSource)
        {
            if (shouldShowScore)
            {
                _gameOverUiView.ShowScore();
                _gameOverUiView.SetScore(score, scoreGoal, cancellationTokenSource);
            }
            else
            {
                _gameOverUiView.HideScore();
            }
            
            _ = _gameOverUiView.Show(cancellationTokenSource);
        }

        public void UpdateScore(int newScore, CancellationTokenSource cancellationTokenSource)
        {
            _inGameUiView.SetCurrentScoreText(newScore, cancellationTokenSource);
        }
        
        public void SwitchToInGameView()
        {
            _inGameUiView.gameObject.SetActive(true);
            _beforeGameUiView.gameObject.SetActive(false);
        }
        
        public void SwitchToBeforeGameView(int currentLevel)
        {
            _beforeGameUiView.SetCurrentLevelText(currentLevel.ToString());
            _beforeGameUiView.gameObject.SetActive(true);
            _inGameUiView.gameObject.SetActive(false);
            _gameOverUiView.Hide();
            _winUiView.gameObject.SetActive(false);
        }

        public void SetStartingValues(int score, int scoreGoal, CancellationTokenSource cancellationTokenSource)
        {
            _inGameUiView.SetCurrentScoreText(score, cancellationTokenSource, false);
            _inGameUiView.SetScoreGoal(scoreGoal);
        }

        public void ShowWinPanel(int winScore, int scoreGoal, CancellationTokenSource cancellationTokenSource)
        {
            _winUiView.gameObject.SetActive(true);
            _winUiView.SetScoreText(winScore, scoreGoal, cancellationTokenSource);
        }

        public void InitExitPoint()
        {
            _inGameUiView.InitExitPoint();
        }
    }
}