using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Dialogue.Editor
{
    public class VariableEditor : ScriptableObject
    {
        [OnInspectorGUI("GUIBefore", "GUIAfter")]
        [LabelText(" ")]
        [ListDrawerSettings(Expanded = true, ShowItemCount = false)]
        [SerializeField]
        private List<Variable> variables = new List<Variable>();
        
        private string searchFilter;
        
        public void UpdateVariables()
        {
            //DialogueDatabase db = DialogueManagerEditor.Instance.Databases.GetSelected();
            DialogueDatabase db = DialogueManager.CurrentDatabase;

            if (db == null || db.Variables == variables)
                return;

            variables = db.Variables;
        }

        [UsedImplicitly]
        private void GUIAfter()
        {
            // Note: Even though this is empty it is required by the variables field
        }

        [UsedImplicitly]
        private void GUIBefore()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                EditorGUI.BeginChangeCheck();
                {
                    searchFilter = EditorGUILayout.TextField(GUIContent.none, searchFilter,
                        SirenixGUIStyles.ToolbarSearchTextField, GUILayout.Width(300));
                    GUILayout.Label(string.Empty, SirenixGUIStyles.ToolbarSearchCancelButton);
                }
                if (EditorGUI.EndChangeCheck())
                {
                }

                DrawMenu();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMenu()
        {
            if (!GUILayout.Button("Menu", "MiniPullDown", GUILayout.Width(56)))
                return;
            
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("New Variable"), false, () => { });
            menu.AddItem(new GUIContent("Sort by name"), false, () => { });
            menu.AddItem(new GUIContent("Sort by id"), false, () => { });
            menu.ShowAsContext();
        }
    }
}