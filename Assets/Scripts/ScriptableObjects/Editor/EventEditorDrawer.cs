using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CustomEditor(typeof(GameEventScriptableObject))]
    public class EventEditorDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameEventScriptableObject e = target as GameEventScriptableObject;
            if (GUILayout.Button("Invoke"))
                e.Invoke();
        }
    }
}