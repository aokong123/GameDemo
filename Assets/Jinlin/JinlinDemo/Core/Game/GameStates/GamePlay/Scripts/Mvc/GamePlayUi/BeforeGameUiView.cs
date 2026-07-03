using TMPro;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi
{
    public class BeforeGameUiView : MonoBehaviour
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private const string PRESS_ANY_INPUT_MOBILE_TEXT = "Tap To Jump\nLong Press To Shoot";
#endif
        private const string PRESS_ANY_INPUT_PC_TEXT = "Press SPACE To Jump\nLong Press To Shoot";
        
        private const string CURRENT_LEVEL_TEXT_FORMAT = "Level {0}";
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private TextMeshProUGUI _pressAnyInputText;

        public void InitEntryPoint()
        {
            var pressAnyInputText = PRESS_ANY_INPUT_PC_TEXT;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            pressAnyInputText = PRESS_ANY_INPUT_MOBILE_TEXT;
#endif
            _pressAnyInputText.text = pressAnyInputText;
        }

        public void SetCurrentLevelText(string text)
        {
            _currentLevelText.text = string.Format(CURRENT_LEVEL_TEXT_FORMAT, text);
        }
    }
}
