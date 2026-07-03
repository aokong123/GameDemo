using System;
using System.Threading;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Scripts.Helpers;
using CoreDomain.Scripts.Services.Logger.Base;
using TMPro;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi
{
    public class GameOverUiView : MonoBehaviour
    {
        private const string SHOW_ANIMATION_CLIP_NAME = "GameOverShow";
        private const string HIDE_ANIMATION_CLIP_NAME = "GameOverHide";
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private const string PRESS_ANY_INPUT_MOBILE_TEXT = "Tap To Restart";
#endif
        private const string PRESS_ANY_INPUT_PC_TEXT = "Press Any Key To Restart";

        [SerializeField] private CountableTextView _scoreCountableTextView;
        [SerializeField] private Animation _animation;
        [SerializeField] private TextMeshProUGUI _pressAnyKeyToRestartText;

        public void InitEntryPoint()
        {
            var pressAnyInputText = PRESS_ANY_INPUT_PC_TEXT;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            pressAnyInputText = PRESS_ANY_INPUT_MOBILE_TEXT;
#endif
            _pressAnyKeyToRestartText.text = pressAnyInputText;
        } 
        public async Awaitable Show(CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                await _animation.PlayAsync(SHOW_ANIMATION_CLIP_NAME, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                LogService.Log("Operation Show GameOverUiView was cancelled");
            }
            catch (Exception e)
            {
                LogService.LogError(e.Message);
                throw;
            }
        }
        
        public void Hide()
        {
            _animation.Play(HIDE_ANIMATION_CLIP_NAME);
        }
        
        public void SetScore(int scoreAchieved, int scoreGoal, CancellationTokenSource cancellationTokenSource)
        {
            _scoreCountableTextView.SetNumber(0);
            _scoreCountableTextView.SetGoalNumber(scoreGoal);
            _scoreCountableTextView.CountToNumber(scoreAchieved, cancellationTokenSource);
        }

        public void ShowScore()
        {
            _scoreCountableTextView.gameObject.SetActive(true);
        }

        public void HideScore()
        {
            _scoreCountableTextView.gameObject.SetActive(false);
        }
    }
}
