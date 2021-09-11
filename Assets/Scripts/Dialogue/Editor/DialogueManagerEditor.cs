using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Misc;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Dialogue.Editor
{
    public class DialogueManagerEditor : OdinMenuEditorWindow // Singleton<DialogueManagerEditor>
    {
        private int iteration;

        private const string DialoguePath = "Assets/Game/Dialogue";
        private const string DatabasePath = "Assets/Game/Dialogue";
        
        private readonly DrawSelected<DialogueEditor> dialogues = new DrawSelected<DialogueEditor>();

        //private DrawVariable drawVariable = new DrawVariable();
        private readonly DrawSelected<VariableEditor> variables = new DrawSelected<VariableEditor>();
        
        private DialogueEditor dialogueEditor;
        private VariableEditor variableEditor;
        private int enumIndex;

        [HideLabel] [ShowInInspector] [EnumToggleButtons] [OnValueChanged("StateChange")]
        private ManagerState managerState;

        private bool treeRebuild;

        public DrawSelected<DialogueDatabase> Databases { get; } = new DrawSelected<DialogueDatabase>();

        protected override void OnEnable()
        {
            dialogueEditor = CreateInstance<DialogueEditor>();
            variableEditor = CreateInstance<VariableEditor>();
        }

        protected override void OnGUI()
        {
            if (treeRebuild && Event.current.type == EventType.Layout)
            {
                ForceMenuTreeRebuild();
                treeRebuild = false;
            }

            SirenixEditorGUI.Title("Dialogue Manager", "Because I overdo everything", TextAlignment.Center, true);
            EditorGUILayout.Space();

            switch (managerState)
            {
                case ManagerState.Database:
                case ManagerState.Quests:
                case ManagerState.Variables:
                case ManagerState.Dialogues:
                    // Note: For some reason on the first pass, DrawEditor causes a null reference exception
                    // Note: so we must wait until the second iteration to call DrawEditor
                    if (iteration >= 1)
                        DrawEditor(enumIndex);
                    break;
            }

            if (iteration < 1)
                iteration++;
            
            base.OnGUI();
        }

        [MenuItem("Window/Dialogue Manager")]
        private static void ShowWindow() => GetWindow<DialogueManagerEditor>(false, "Dialogue Manager");

        protected override void Initialize()
        {
            Databases.SetPath(DatabasePath);
            dialogues.SetPath(DialoguePath);
            //drawVariable.FindMyObject();
        }

        private Dialogue currentDialogue;
        
        protected override void DrawEditors()
        {
            switch (managerState)
            {
                case ManagerState.Database:
                    DialogueDatabase db = MenuTree.Selection.SelectedValue as DialogueDatabase;
                    Selection.activeObject = db;
                    Databases.SetSelected(db);
                    DialogueManager.CurrentDatabase = db;
                    break;
                case ManagerState.Player:
                    DrawEditor(enumIndex);
                    break;
                case ManagerState.Npcs:
                    DrawEditor(enumIndex);
                    break;
                case ManagerState.Quests:
                    break;
                case ManagerState.Variables:
                    variables.SetSelected(variableEditor);
                    variableEditor.UpdateVariables();
                    break;
                case ManagerState.Dialogues:
                    Dialogue dg = MenuTree.Selection.SelectedValue as Dialogue;
                    if (dg != currentDialogue)
                    {
                        Selection.activeObject = dg;
                        dialogueEditor.SelectedDialogue = dg;
                        currentDialogue = dg;
                    }

                    dialogues.SetSelected(dialogueEditor);
                    
                    break;
            }
            
            DrawEditor((int) managerState);
        }

        protected override IEnumerable<object> GetTargets()
        {
            var targets = new List<object>
            {
                Databases,
                null,
                null,
                null,
                variables,
                dialogues,
                base.GetTarget()
            };


            enumIndex = targets.Count - 1;

            return targets;
        }

        protected override void DrawMenu()
        {
            switch (managerState)
            {
                case ManagerState.Database:
                case ManagerState.Quests:
                //case ManagerState.Variables:
                case ManagerState.Dialogues:
                    base.DrawMenu();
                    break;
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            switch (managerState)
            {
                case ManagerState.Database:
                    tree.AddAllAssetsAtPath("Databases", DatabasePath, typeof(DialogueDatabase));
                    break;
                case ManagerState.Quests:
                    break;
                // case ManagerState.Variables:
                //     break;
                case ManagerState.Dialogues:
                    tree.AddAllAssetsAtPath("Dialogues", DialoguePath, typeof(Dialogue));
                    break;
            }

            return tree;
        }

        [UsedImplicitly]
        private void StateChange()
        {
            treeRebuild = true;
        }

        private enum ManagerState
        {
            Database,
            Player,
            Npcs,
            Quests,
            Variables,
            Dialogues
        }

        protected override void OnDestroy()
        {
            if (Databases.GetSelected() != null)
                Utils.MarkAsDirty(Databases.GetSelected());

            Selection.objects = null;
        }
    }

    [Serializable]
    public class DrawSelected<T> where T : ScriptableObject
    {
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        [DisableContextMenu()]
        [SerializeField] private T selected;

        [LabelWidth(100)] [PropertyOrder(-1)] [SerializeField] [ShowIf("@selected.GetType() != typeof(VariableEditor)")]
        [HorizontalGroup("CreateNew")]
        private string nameForNew;

        private string path;

        [Button] [ShowIf("@selected.GetType() != typeof(VariableEditor)")]
        [HorizontalGroup("CreateNew")]
        public void CreateNew()
        {
            if (nameForNew == "")
                return;

            var newItem = ScriptableObject.CreateInstance<T>();
            newItem.name = "New " + typeof(T); //May not need

            if (path == "")
                path = "Assets/";

            AssetDatabase.CreateAsset(newItem, path + "\\" + nameForNew + ".asset");
            AssetDatabase.SaveAssets();

            nameForNew = "";
        }

        [Button] [ShowIf("@selected.GetType() != typeof(VariableEditor)")]
        [HorizontalGroup("CreateNew")]
        public void DeleteSelected()
        {
            if (selected == null)
                return;

            var assetPath = AssetDatabase.GetAssetPath(selected);
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
        }

        public void SetSelected(object item)
        {
            var attempt = item as T;

            if (attempt != null)
                selected = attempt;
        }

        public T GetSelected()
        {
            return selected;
        }

        public void SetPath(string newPath) => path = newPath;
    }
}