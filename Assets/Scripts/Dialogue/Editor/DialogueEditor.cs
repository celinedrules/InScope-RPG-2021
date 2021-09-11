using System;
using System.Linq;
using Dialogue.GUI;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Graphs;
using UnityEngine;

namespace Dialogue.Editor
{
    public class DialogueEditor : ScriptableObject
    {
        [NonSerialized] private GUIStyle style;
        [NonSerialized] private DialogueNode draggingNode;
        [NonSerialized] private DialogueNode creatingNode;
        [NonSerialized] private DialogueNode linkingParent;
        [NonSerialized] private DialogueNode linkedChild;
        [NonSerialized] private bool draggingCanvas;
        [NonSerialized] private Vector2 draggingCanvasOffset;
        [NonSerialized] private Vector2 contextMenuPosition;
        [NonSerialized] private string[] contextMenuItems;
        [NonSerialized] private bool showContextMenu;
        [NonSerialized] private bool showNodeContextMenu;
        [NonSerialized] private bool showLinkContextMenu;
        [NonSerialized] private bool isMakingLink;

        private Vector2 scrollPosition;
        private const float CanvasSize = 4000;
        private Material linkArrowMaterial;
        private bool dragBox;
        private Vector2 dragBoxStartPos;
        private Rect dragRect;

        public Dialogue SelectedDialogue { get; set; }

        // Note : Enable if using editor window
        // [MenuItem("Windows/Dialogue Editor")]
        // private static void ShowWindow() => GetWindow<DialogueEditor>(false, "Dialogue Editor");

        /// <summary>
        /// Opens the Dialogue Editor when a dialogue asset is double clicked
        /// </summary>
        [OnOpenAsset(1)]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID)?.GetType() != typeof(Dialogue))
                return false;

            // Note : Enable if using editor window
            //ShowWindow();

            return true;
        }

        /// <summary>
        /// Called when the new window is opened
        /// </summary>
        private void Awake()
        {
            OnSelectionChanged();
        }

        /// <summary>
        /// Called when the window is loaded
        /// </summary>
        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            
            contextMenuItems = new[]
            {
                "Add New Node",
                "Add Child Node",
                "Delete Node",
                "Make Link"
            };
        }

        /// <summary>
        /// Triggered when the currently active/selected dialogue has changed
        /// </summary>
        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;

            if (newDialogue == null)
                return;

            SelectedDialogue = newDialogue;
        }

        /// <summary>
        /// Custom editor GUI to draw the dialogue editor
        /// </summary>
        [OnInspectorGUI]
        private void OnGUI()
        {
            // Check to see if a dialogue is selected in the project view
            if (SelectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                // Create and set the position of an automatic scroll view
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                {
                    //Reserve layout space for a rectangle with a fixed content area.
                    GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
                    // Draw the background grid of the editor
                    Grid.DrawGrid(20, 0.2f, Color.gray, new Vector2(CanvasSize, CanvasSize), Vector2.zero);
                    Grid.DrawGrid(100, 0.4f, Color.gray, new Vector2(CanvasSize, CanvasSize), Vector2.zero);

                    // Loop through all the nodes in the dialogue and draw the connection between the
                    // parent and child
                    foreach (var node in SelectedDialogue.Nodes)
                        DrawConnections(node);

                    // Draw all he nodes in the dialogue
                    foreach (var node in SelectedDialogue.Nodes)
                        DrawNode(node);
                }
                EditorGUILayout.EndScrollView();

                if (Event.current.type == EventType.Repaint)
                {
                    // Create a new node
                    if (creatingNode != null)
                    {
                        SelectedDialogue.CreateNode(creatingNode, Vector2.zero);
                        creatingNode = null;
                        draggingNode = null;
                    }

                    // Delete a node
                    // if (deletingNode != null)
                    // {
                    //     selectedDialogue.DeleteNode(deletingNode);
                    //     deletingNode = null;
                    // }
                }
            }

            // Show a context menu
            if (showContextMenu || showNodeContextMenu || showLinkContextMenu)
                ShowContextMenu();

            // Note : Enable if using editor window
            //Repaint();
        }

        /// <summary>
        /// Shows the appropriate context menu
        /// </summary>
        private void ShowContextMenu()
        {
            GenericMenu contextMenu = new GenericMenu();
            
            if (showContextMenu)    // Canvas context menu
            {
                // Creates a new node
                contextMenu.AddItem(new GUIContent(contextMenuItems[0]), false,
                    () => SelectedDialogue.CreateNode(null, contextMenuPosition));
            }
            else if (showNodeContextMenu)   // Node context menu
            {
                // Add child node
                contextMenu.AddItem(new GUIContent(contextMenuItems[1]), false,
                    () => creatingNode = draggingNode);

                // Delete the node
                if (draggingNode != SelectedDialogue.RootNode)
                    contextMenu.AddItem(new GUIContent(contextMenuItems[2]), false, DeleteSelectedNodes);
                // The node is the root node so we want the delete menu item to be disabled
                else
                    contextMenu.AddDisabledItem(new GUIContent(contextMenuItems[2]));

                // Create a link between nodes
                contextMenu.AddItem(new GUIContent(contextMenuItems[3]), false,
                    MakeLink);
            }
            else if (showLinkContextMenu)   // Link context men
            {
                // Delete the link
                contextMenu.AddItem(new GUIContent("Delete Link"), false,
                    DeleteLink);
            }

            contextMenu.ShowAsContext();
        }

        /// <summary>
        /// Deleted all selected nodes
        /// </summary>
        private void DeleteSelectedNodes()
        {
            var nodesToDelete = SelectedDialogue.Nodes.Where(x => x.IsSelected).ToArray();
            
            foreach (var nodeToDelete in nodesToDelete)
                SelectedDialogue.DeleteNode(nodeToDelete);
        }
        
        /// <summary>
        /// Deletes a link
        /// </summary>
        private void DeleteLink()
        {
            linkedChild.Parent = null;
            linkingParent.Children.Remove(linkedChild.name);

            linkedChild = null;
            linkingParent = null;
        }

        /// <summary>
        /// Makes a link between nodes
        /// </summary>
        private void MakeLink()
        {
            linkingParent = draggingNode;
            isMakingLink = linkingParent != null;
        }

        /// <summary>
        /// Complete the link from parent to child
        /// </summary>
        private void FinishMakingLink()
        {
            if (linkingParent != null && linkedChild != null && linkingParent != linkedChild)
                linkingParent.Children.Add(linkedChild.name);

            isMakingLink = false;
            linkedChild = null;
            linkingParent = null;
        }

        /// <summary>
        /// Handle all input events
        /// </summary>
        private void ProcessEvents() => ProcessEvents(Vector3.zero, Vector3.zero);
        
        /// <summary>
        /// Handle all input events. Start and end are only used when creating a link between nodes.
        /// If we don't need to create a link, then start and end should be Vector2.zero
        /// </summary>
        /// <param name="start">The start position of the link</param>
        /// <param name="end">The end position of the link</param>
        private void ProcessEvents(Vector3 start, Vector3 end)
        {
            if (Application.isPlaying)
                return;

            Event current = Event.current;
            
            ProcessDragEvents(current);

            switch (current.type)
            {
                case EventType.MouseDown when current.button == 1:
                {
                    ProcessContextEvents(start, end, current);
                    break;
                }
                case EventType.MouseDown when isMakingLink:
                {
                    FinishLink(current);
                    break;
                }
                default:
                {
                    HideContextMenus(current);
                    break;
                }
            }
        }

        /// <summary>
        /// Handle any events related to context menus. Start and end are only used when creating a link between nodes.
        /// If we don't need to create a link, then start and end should be Vector2.zero
        /// </summary>
        /// <param name="start">The start position of the link</param>
        /// <param name="end">The ebd position of the link</param>
        /// <param name="current">The current GUI event</param>
        private void ProcessContextEvents(Vector3 start, Vector3 end, Event current)
        {
            if (IsPointOnLineSegment(current.mousePosition, start, end)) // Check to see if we clicked on a link
            {
                showLinkContextMenu = true;
                showContextMenu = false;

                linkingParent = GetNodeAtPoint(start);
                linkedChild = GetNodeAtPoint(end);
            }
            else if (!showLinkContextMenu) // Check if we want to show the canvas or node context menu
            {
                // Note : Enable scrollPosition if using editor window
                draggingNode = GetNodeAtPoint(current.mousePosition /*+ scrollPosition*/);
                contextMenuPosition = current.mousePosition;

                if (draggingNode == null)
                {
                    showContextMenu = true;
                    showNodeContextMenu = false;
                }
                else
                {
                    showNodeContextMenu = true;
                    showContextMenu = false;
                }
            }
        }

        /// <summary>
        /// Hide any context menus
        /// </summary>
        /// <param name="current">The current GUI event</param>
        private void HideContextMenus(Event current)
        {
            if (current.type == EventType.MouseMove)
                return;

            showContextMenu = false;
            showNodeContextMenu = false;
            showLinkContextMenu = false;
        }

        /// <summary>
        /// Finish creating the link between parent and child nodes
        /// </summary>
        /// <param name="current">The current GUI event</param>
        private void FinishLink(Event current)
        {
            linkedChild = GetNodeAtPoint(current.mousePosition/* + scrollPosition*/);
            
            if (linkedChild != null)
                FinishMakingLink();
        }

        /// <summary>
        /// Handle events relating to dragging
        /// </summary>
        /// <param name="current">The current GUI event</param>
        private void ProcessDragEvents(Event current)
        {
            switch (current.type)
            {
                // Starts the creation of a selection box
                case EventType.MouseDrag when dragBox == false && draggingNode == null:
                    dragBox = true;
                    dragBoxStartPos = current.mousePosition;
                    break;
                // Ends the creation of the selection box
                case EventType.MouseUp when dragBox:
                    dragBox = false;
                    // Get Selected nodes contained in the selection box
                    SelectNodes();
                    break;
                // Based on the current mouse button either select a node or drag the canvas
                case EventType.MouseDown when draggingNode == null:
                    switch (current.button)
                    {
                        case 0: SelectNode(current);
                            break;
                        case 2: DragCanvas();
                            break;
                    }
                    break;
                // Drag the selected node
                case EventType.MouseDrag when draggingNode != null && draggingNode.CanDrag:
                    DragNode(current);
                    break;
                // Adjust the scroll view position when dragging the canvas
                case EventType.MouseDrag when draggingCanvas:
                    scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                    UnityEngine.GUI.changed = true;
                    break;
                // Stop dragging the selected node
                case EventType.MouseUp when draggingNode != null:
                    draggingNode = null;
                    break;
                // Stop dragging the canvas
                case EventType.MouseUp when draggingCanvas:
                    draggingCanvas = false;
                    break;
            }

            // If the middle mouse button is down, we don't want to create a selection box
            if(current.type == EventType.Layout)
                if (current.button == 2)
                    dragBox = false;
            
            if (Event.current.type != EventType.Repaint)
                return;
            
            if (!dragBox)
                return;
            
            Color oldColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.25f);
            
            // Create the rect for the selection box
            dragRect = new Rect(dragBoxStartPos.x, dragBoxStartPos.y,
                current.mousePosition.x - dragBoxStartPos.x,
                current.mousePosition.y - dragBoxStartPos.y);

            // Draw the selection box
            UnityEngine.GUI.Box(dragRect, "");

            UnityEngine.GUI.color = oldColor;
        }

        /// <summary>
        /// Drags a node
        /// </summary>
        /// <param name="current">The current GUI event</param>
        private void DragNode(Event current)
        {
            foreach (var dialogueNode in SelectedDialogue.Nodes)
            {
                if (dialogueNode.IsSelected)
                {
                    dialogueNode.SetPosition(current.mousePosition + dialogueNode.DragOffset,
                        SelectedDialogue.SnapToGrid ? SelectedDialogue.GridSnapSize : 1);
                }
            }
            
            UnityEngine.GUI.changed = true;
        }

        /// <summary>
        /// Drags the canvas
        /// </summary>
        private void DragCanvas()
        {
            draggingCanvas = true;
            draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
        }

        /// <summary>
        /// Selects a node
        /// </summary>
        /// <param name="current">The current GUI event</param>
        private void SelectNode(Event current)
        {
            // Remove focus from any nodes
            UnityEngine.GUI.FocusControl(null);
            
            // Check if a node is under the current mouse
            // Note : Enable scrollPosition if using editor window
            draggingNode = GetNodeAtPoint(current.mousePosition /*+ scrollPosition*/);

            if (draggingNode != null)
            {
                if (!draggingNode.IsSelected)
                {
                    // If we don't want to multiselect nodes, deselect all the nodes
                    if(current.modifiers != EventModifiers.Control)
                        foreach (var node in SelectedDialogue.Nodes)
                            node.IsSelected = false;
                }

                // select the node under the mouse
                draggingNode.IsSelected = true;
                Selection.activeObject = draggingNode;
            }
            else
            {
                // We clicked either on the canvas or a link so deselect all nodes
                foreach (var node in SelectedDialogue.Nodes)
                    node.IsSelected = false;
                
                // Set the active object to the dialogue making the properties visible in the inspector
                
                Selection.activeObject = SelectedDialogue;
            }
            
            if (showContextMenu || showNodeContextMenu)
                return;

            if (draggingNode == null || !draggingNode.CanDrag)
                return;

            // Loop through and set the drag offset for the nodes. Used for multiselect.
            foreach (var dialogueNode in SelectedDialogue.Nodes)
                dialogueNode.DragOffset = dialogueNode.Rect.position - Event.current.mousePosition;
        }

        /// <summary>
        /// Gets a node at the specified point
        /// </summary>
        /// <param name="point">The point to check for a node</param>
        /// <returns>A node at the specified point</returns>
        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach (var node in SelectedDialogue.Nodes)
                if (node.Rect.Contains(point))
                    foundNode = node;

            return foundNode;
        }

        /// <summary>
        /// Select multiple nodes using a selection box
        /// </summary>
        private void SelectNodes()
        {
            foreach (var dialogueNode in SelectedDialogue.Nodes)
            {
                Vector2 topLeft = new Vector2(dialogueNode.Rect.x, dialogueNode.Rect.y);
                Vector2 bottomRight = new Vector2(dialogueNode.Rect.x + dialogueNode.Rect.width,
                    dialogueNode.Rect.y + dialogueNode.Rect.height);

                if (dragRect.Contains(topLeft) && dragRect.Contains(bottomRight))
                    dialogueNode.IsSelected = true;
                else
                    dialogueNode.IsSelected = false;
            }
            
        }
        
        /// <summary>
        /// Draws a node in the editor
        /// </summary>
        /// <param name="node">The node to draw</param>
        private void DrawNode(DialogueNode node)
        {
            style = DialogueEditorGUI.gui.GetStyle("dialogue");
            style.stretchHeight = false;
            style.padding.bottom = 0;

            GUILayout.BeginArea(new Rect(node.Rect.x - 2, node.Rect.y - 2, 150 + 12, 40 + 12));
            {
                var nodeColor = FindObjectOfType<PlayerConversant>().PlayerName == node.Speaker
                    ? GetNodeColor(true)
                    : GetNodeColor(false);

                GUIStyle nodeStyle = new GUIStyle(Styles.GetNodeStyle("node", nodeColor, node.IsSelected))
                {
                    normal = {textColor = EditorGUIUtility.isProSkin ? new Color(0.9f, 0.9f, 0.9f) : Color.black},
                    wordWrap = false,
                    alignment = TextAnchor.MiddleCenter
                };

                string txt = node.Text.Substring(0, Mathf.Min(node.Text.Length, 20));

                if (txt == "")
                    txt = "[Empty]";

                if (node.Children.Count == 0)
                {
                    if (txt.Length > 14)
                        txt = txt.Substring(0, Math.Max(txt.Length - 6, 0)) + " [END]";
                    else
                        txt += " [END]";
                }
                else if (SelectedDialogue.RootNode == node)
                {
                    if (txt.Length > 12)
                        txt = txt.Substring(0, Math.Max(txt.Length - 8, 0)) + " [START]";
                    else
                        txt += " [Start]";
                }

                var nodeRect = new Rect(2, 2, 150, 40);

                if (node.IsSelected) // If the node is selected draw a border around it
                {
                    var selectionRect = new Rect(nodeRect.x + 1, nodeRect.y, nodeRect.width - 2, nodeRect.height - 1);
                    UnityEngine.GUI.Box(selectionRect, string.Empty,
                        Styles.GetNodeStyle("node", Styles.Color.Blue, true));
                }

                // Draw the node
                UnityEngine.GUI.Box(nodeRect, txt, nodeStyle);

                if (Event.current.type == EventType.Repaint)
                    node.CanDrag = true;
            }
            GUILayout.EndArea();
            
            ProcessEvents();
        }

        /// <summary>
        /// Draw an existing connection from the output button to the child input button
        /// </summary>
        private void DrawConnections(DialogueNode node)
        {
            foreach (DialogueNode childNode in SelectedDialogue.GetAllChildren(node))
            {
                Vector3 start = new Vector3(node.Rect.center.x, node.Rect.center.y, 0);
                Vector3 end = new Vector3(childNode.Rect.center.x, childNode.Rect.center.y, 0);

                if (Event.current.type == EventType.Repaint)
                    DrawLink(start, end);
                else
                    ProcessEvents(start, end);
            }

            if (isMakingLink)
                DrawNewLinkConnector();
        }

        /// <summary>
        /// Checks if there is a link at the specified point
        /// </summary>
        /// <param name="point">The point to check for the link</param>
        /// <param name="start">The start position of the link</param>
        /// <param name="end">The end position of the link</param>
        /// <returns></returns>
        private bool IsPointOnLineSegment(Vector2 point, Vector3 start, Vector3 end)
        {
            const float tolerance = 10f;
            float minX = Mathf.Min(start.x, end.x);
            float minY = Mathf.Min(start.y, end.y);
            float width = Mathf.Abs(start.x - end.x);
            float height = Mathf.Abs(start.y - end.y);
            float midX = minX + (width / 2);

            if ((width <= tolerance) && (Mathf.Abs(point.x - midX) <= tolerance) && (minY <= point.y) &&
                (point.y <= minY + height))
                return true; // Special case: vertical line.

            if ((minX < point.x && point.x < (minX + width)) && (Mathf.Abs(start.y - end.y) <= 2) &&
                (Mathf.Abs(point.y - end.y) <= 2))
                return true; // Special case: horizontal line.
            Rect boundingRect = new Rect(minX, minY, width, height);

            if (boundingRect.Contains(point))
            {
                float slope = (end.y - start.y) / (end.x - start.x);
                float yIntercept = -(slope * start.x) + start.y;
                float distanceFromLine = Mathf.Abs(point.y - (slope * point.x + yIntercept));
                return (distanceFromLine <= tolerance);
            }

            return false;
        }

        /// <summary>
        /// Draw a link between the parent node and the child node
        /// </summary>
        /// <param name="start">The start position of the link</param>
        /// <param name="end">The ned position of the link</param>
        private void DrawLink(Vector3 start, Vector3 end)
        {
            Vector3 cross = Vector3.Cross((start - end).normalized, Vector3.forward);
            Texture2D connectionTexture = (Texture2D) Styles.connectionTexture.image;
            Handles.color = GetConnectionColor();
            Handles.DrawAAPolyLine(connectionTexture, 4, start, end);
            Vector3 diff = (end - start);
            Vector3 direction = diff.normalized;
            Vector3 mid = ((0.5f * diff) + start) - (0.5f * cross);
            Vector3 center = mid + direction;
            
            DrawArrow(cross, direction, center);
        }

        /// <summary>
        /// Draws a new link from the parent node to the current mouse position
        /// </summary>
        private void DrawNewLinkConnector()
        {
            if (!isMakingLink || linkingParent == null)
                return;

            Vector3 start = new Vector3(linkingParent.Rect.center.x, linkingParent.Rect.center.y, 0);

            if (linkedChild != null && Event.current.isMouse)
            {
                if (!linkedChild.Rect.Contains(Event.current.mousePosition))
                    linkedChild = null;
            }

            Vector3 end = (linkedChild != null)
                ? new Vector3(linkedChild.Rect.center.x, linkedChild.Rect.center.y, 0)
                : new Vector3(Event.current.mousePosition.x, Event.current.mousePosition.y, 0);

            DrawLink(start, end);
        }

        /// <summary>
        /// Draws an arrow on the link indicating the direction from parent to child
        /// </summary>
        /// <param name="cross">A 90 degree angle between the links middle and Vector3.forward</param>
        /// <param name="direction">The direction of the arrow</param>
        /// <param name="center">The center point of the link</param>
        private void DrawArrow(Vector3 cross, Vector3 direction, Vector3 center)
        {
            const float sideLength = 6f;

            Vector3[] vertices =
            {
                center + (direction * sideLength),
                (center - (direction * sideLength)) + (cross * sideLength),
                (center - (direction * sideLength)) - (cross * sideLength)
            };

            UseLinkArrowMaterial();
            Handles.DrawAAConvexPolygon(vertices);
            // GL.Begin(vertices.Length + 1);
            // GL.Color(GetConnectionColor());
            //
            // foreach (var t in vertices)
            //     GL.Vertex(t);
            //
            // GL.End();
        }

        /// <summary>
        /// Get or create the material used on the link
        /// </summary>
        private void UseLinkArrowMaterial()
        {
            if (linkArrowMaterial == null)
            {
                var shader = Shader.Find("Lines/Colored Blended") ??
                             Shader.Find("Legacy Shaders/Transparent/Diffuse") ?? Shader.Find("Transparent/Diffuse");
                if (shader == null)
                    return;

                linkArrowMaterial = new Material(shader);
            }

            linkArrowMaterial.SetPass(0);
        }

        /// <summary>
        /// Converts the rgb color to a Graphs.Styles.Color for use in drawing nodes.
        /// </summary>
        /// <param name="isPlayerSpeaking">Is this a player node or an npc node</param>
        /// <returns>A Graphs.Styles.Color</returns>
        private Styles.Color GetNodeColor(bool isPlayerSpeaking)
        {
            Color color = isPlayerSpeaking
                ? SelectedDialogue.PlayerColor.GetPixel(1, 1)
                : SelectedDialogue.NpcColor.GetPixel(1, 1);

            if (color == Color.cyan)
                return Styles.Color.Aqua;
            if (color == Color.blue)
                return Styles.Color.Blue;
            if (color == Color.grey)
                return Styles.Color.Grey;
            if (color == Color.green)
                return Styles.Color.Green;
            if (color == new Color(1, 1, 0, 1))
                return Styles.Color.Yellow;
            if (color == Color.red)
                return Styles.Color.Red;

            return Styles.Color.Orange;
        }

        /// <summary>
        /// Gets the color used for drawing links
        /// </summary>
        /// <returns></returns>
        private Color GetConnectionColor() => SelectedDialogue.NodeConnectionColor.GetPixel(1, 1);
    }
}
