using UnityEngine;

namespace CoreDomain.GameDomain.Scripts.Services.Levels.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Levels", menuName = "Jinlin/Game/Levels/Levels")]
    public class LevelsData : ScriptableObject
    {
        public LevelData[] LevelsByOrder;
    }
}
