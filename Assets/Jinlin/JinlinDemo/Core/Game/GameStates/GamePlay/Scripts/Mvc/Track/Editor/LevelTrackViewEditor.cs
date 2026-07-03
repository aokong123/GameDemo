using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Bubble;
using UnityEditor;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Track.Editor
{
    [CustomEditor(typeof(LevelTrackView))]
    public class LevelTrackViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var trackView = (LevelTrackView)target;

            if (GUILayout.Button("Locate All Views"))
            {
                Undo.RecordObject(trackView, "Locate All Views");

                trackView.BubbleViews = trackView.transform.GetComponentsInChildren<BubblesView>();
                trackView.BalloonViews = trackView.transform.GetComponentsInChildren<BalloonView>();

                EditorUtility.SetDirty(trackView);
            }
        }
    }
}