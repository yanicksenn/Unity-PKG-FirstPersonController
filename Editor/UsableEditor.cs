using UnityEditor;
using YanickSenn.Controller.FirstPerson.Hand;

namespace YanickSenn.Controller.FirstPerson.Editor {
    [CustomEditor(typeof(Usable), true)]
    public class UsableEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();
            
            DrawPropertiesExcluding(serializedObject, "m_Script", "onUseEvent");
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onUseEvent"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
