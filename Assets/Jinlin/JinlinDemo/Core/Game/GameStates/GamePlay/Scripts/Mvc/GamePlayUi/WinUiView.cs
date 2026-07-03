using System.Threading;
using CoreDomain.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi
{
    public class WinUiView : MonoBehaviour
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private const string PRESS_ANY_INPUT_MOBILE_TEXT = "Tap To Continue";
#endif
        private const string PRESS_ANY_INPUT_PC_TEXT = "Press Any Key To Continue";

        [SerializeField] private CountableTextView _scoreCountable;
        [SerializeField] private TextMeshProUGUI _pressAnyInputText;

        public void InitEntryPoint()
        {
            var pressAnyInputText = PRESS_ANY_INPUT_PC_TEXT;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            pressAnyInputText = PRESS_ANY_INPUT_MOBILE_TEXT;
#endif
            _pressAnyInputText.text = pressAnyInputText;
        }
        
        public void SetScoreText(int scoreAchieved, int scoreGoal, CancellationTokenSource cancellationTokenSource)
        {
            _scoreCountable.SetNumber(0);
            _scoreCountable.SetGoalNumber(scoreGoal);
            _scoreCountable.CountToNumber(scoreAchieved, cancellationTokenSource);
        }
    }
}
