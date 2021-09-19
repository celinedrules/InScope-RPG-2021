using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dialogue.Attributes;
using Dialogue.GUI;
using Misc;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace Dialogue
{
    public enum ColorType
    {
        Player,
        Npc,
        Connection
    }

    [HideMonoScript]
    [CreateAssetMenu(fileName = nameof(Dialogue), menuName = nameof(Dialogue))]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [Title("Dialogue", Bold = true)] [SerializeField, LabelText("ID"), ShowOnly]
        private string id;

        [SerializeField]
        private string title;

        [PropertySpace(SpaceBefore = 0, SpaceAfter = 5), SerializeField]
        private string description;

        [FoldoutGroup("Nodes")]
        [LabelText("Current Nodes"), ListDrawerSettings(Expanded = false), Indent()]
        [ListDrawerSettings(Expanded = true, HideAddButton = true)]
        [SerializeField, ReadOnly]
        private List<DialogueNode> nodes = new List<DialogueNode>();

        [SerializeField, HideInInspector]
        private Texture2D playerColor;

        [SerializeField, HideInInspector]
        private Texture2D npcColor;

        [SerializeField, HideInInspector]
        private Texture2D nodeConnectionColor;

        private bool snapToGrid;
        private int gridSnapSize = 1;
        private GUIStyle btnStyle;
        private readonly Vector2 newNodeOffset = new Vector2(250, 0);
        private readonly Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        public List<DialogueNode> Nodes => nodes;
        public DialogueNode RootNode => nodes[0];

        public Texture2D PlayerColor
        {
            get => playerColor;
            set
            {
                playerColor = value;
                Utils.MarkAsDirty(this);
            }
        }

        public Texture2D NpcColor
        {
            get => npcColor;
            set
            {
                npcColor = value;
                Utils.MarkAsDirty(this);
            }
        }

        public Texture2D NodeConnectionColor
        {
            get => nodeConnectionColor;
            set
            {
                nodeConnectionColor = value;
                Utils.MarkAsDirty(this);
            }
        }

        public bool SnapToGrid
        {
            get => snapToGrid;
            private set
            {
                snapToGrid = value;
                Utils.MarkAsDirty(this);
            }
        }

        public int GridSnapSize
        {
            get => gridSnapSize;
            private set
            {
                gridSnapSize = value;
                Grid.GridSnapSize = gridSnapSize;
                Utils.MarkAsDirty(this);
            }
        }

        public string ID
        {
            get => id;
            set
            {
                id = value;
                Utils.MarkAsDirty(this);
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                Utils.MarkAsDirty(this);
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                Utils.MarkAsDirty(this);
            }
        }

        private void Awake() => OnValidate();

        private void OnValidate()
        {
            EditorApplication.delayCall -= AssetDatabase.SaveAssets;
            EditorApplication.delayCall += AssetDatabase.SaveAssets;
            
            if (ID == "")
                ID = Guid.NewGuid().ToTinyUuid();

            nodeLookup.Clear();

            foreach (var node in nodes)
            {
                if (node != null)
                    nodeLookup[node.name] = node;
            }

            if (PlayerColor == null)
                PlayerColor = DialogueEditorGUI.orangeButton;

            if (NpcColor == null)
                NpcColor = DialogueEditorGUI.blueButton;

            if (NodeConnectionColor == null)
                NodeConnectionColor = DialogueEditorGUI.greyButton;
        }

        [FoldoutGroup("Dialogue Settings")]
        [OnInspectorGUI]
        private void SelectColors()
        {
            GUILayout.BeginVertical();
            {
                SnapToGrid = EditorGUILayout.Toggle("     Snap To Grid", SnapToGrid);
                GridSnapSize = EditorGUILayout.IntField("     Grid Snap Size", GridSnapSize);

                btnStyle = new GUIStyle
                {
                    margin = new RectOffset(0, 20, 0, 3),
                };

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("     Player Color", GUILayout.Width(EditorGUIUtility.labelWidth));

                    if (GUILayout.Button("", btnStyle, GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                    {
                        ColorSelector s = new ColorSelector(this, ColorType.Player);
                        PopupWindow.Show(Rect.zero, s);
                    }

                    TileTexture(PlayerColor, new Rect(0, 0, 20, 20), GUILayoutUtility.GetLastRect(),
                        ScaleMode.StretchToFill);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("     NPC Color", GUILayout.Width(EditorGUIUtility.labelWidth));

                    if (GUILayout.Button("", btnStyle, GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                    {
                        ColorSelector s = new ColorSelector(this, ColorType.Npc);
                        PopupWindow.Show(Rect.zero, s);
                    }

                    TileTexture(NpcColor, new Rect(0, 0, 20, 20), GUILayoutUtility.GetLastRect(),
                        ScaleMode.StretchToFill);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("     Connection Color", GUILayout.Width(EditorGUIUtility.labelWidth));

                    if (GUILayout.Button("", btnStyle, GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                    {
                        ColorSelector s = new ColorSelector(this, ColorType.Connection);
                        PopupWindow.Show(Rect.zero, s);
                    }

                    TileTexture(NodeConnectionColor, new Rect(0, 0, 20, 20), GUILayoutUtility.GetLastRect(),
                        ScaleMode.StretchToFill);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void TileTexture(Texture texture, Rect tile, Rect areaToFill, ScaleMode scaleMode)
        {
            for (float y = areaToFill.y; y < areaToFill.y + areaToFill.height; y += tile.height)
            {
                for (float x = areaToFill.x; x < areaToFill.x + areaToFill.width; x += tile.width)
                {
                    tile.x = x;
                    tile.y = y;
                    UnityEngine.GUI.DrawTexture(tile, texture, scaleMode);
                }
            }
        }

        private void AddNode(DialogueNode newNode)
        {
            newNode.Text = "";
            nodes.Add(newNode);
            OnValidate();

            foreach (DialogueNode node in nodes)
                node.IsSelected = false;
            
            newNode.IsSelected = true;
            
            Save();
        }

        private DialogueNode MakeNode(DialogueNode parent, Vector2 position)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            newNode.SetSize(140, 40);
            if (parent != null)
            {
                parent.AddChild(newNode.name);
                newNode.Parent = parent;
                newNode.IsPlayerSpeaking = !parent.IsPlayerSpeaking;

                newNode.SetPosition(parent.Rect.position + newNodeOffset, Grid.GridSnapSize);

                if (parent.Children.Count > 1)
                    parent.SetSize(new Rect(parent.Rect.x, parent.Rect.y, 150, 40));
            }
            else
            {
                newNode.SetPosition(position, Grid.GridSnapSize);
            }

            return newNode;
        }

#if UNITY_EDITOR
        private void CleanDanglingChildren(DialogueNode node)
        {
            foreach (var dialogueNode in nodes)
                dialogueNode.RemoveChild(node.name);
        }

        private void Save() => AssetDatabase.SaveAssets();
#endif

        public bool IsRootNode(DialogueNode node) => node == nodes[0];
        public int GetChildCount(DialogueNode parentNode) => parentNode.Children.Count;
        public DialogueNode GetChildNode(DialogueNode parentNode, int index) => nodeLookup.ElementAt(index).Value;

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.Children)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (node.IsPlayerSpeaking)
                {
                    yield return node;
                }
            }
        }


        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (!node.IsPlayerSpeaking)
                {
                    yield return node;
                }
            }
        }


        // public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode) =>
        //     parentNode.Children.Select(childID => nodeLookup[childID]).ToList();
        //
        // public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode) =>
        //     GetAllChildren(currentNode).Where(dialogueNode => dialogueNode.IsPlayerSpeaking);
        //
        // public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode) =>
        //     GetAllChildren(currentNode).Where(dialogueNode => !dialogueNode.IsPlayerSpeaking);

#if UNITY_EDITOR
        public DialogueNode CreateNode(DialogueNode parent, Vector2 position)
        {
            var newNode = MakeNode(parent, position);

            Undo.RegisterCreatedObjectUndo(newNode, "Create Dialogue Node");
            Undo.RecordObject(this, "Add Dialogue Node");

            AddNode(newNode);

            return newNode;
        }

        public void DeleteNode(DialogueNode node)
        {
            Undo.RecordObject(this, "Delete Dialogue Node");
            nodes.Remove(node);
            OnValidate();
            CleanDanglingChildren(node);
            Undo.DestroyObjectImmediate(node);
            Save();
        }

        public void OnBeforeSerialize()
        {
            if (nodes.Count == 0)
            {
                var newNode = MakeNode(null, Vector2.zero);
                AddNode(newNode);
            }

            OnValidate();

            if (AssetDatabase.GetAssetPath(this) == "")
                return;

            foreach (var node in nodes.Where(node => AssetDatabase.GetAssetPath(node) == ""))
                AssetDatabase.AddObjectToAsset(node, this);
        }
#endif

        public void OnAfterDeserialize()
        {
        }
    }
}