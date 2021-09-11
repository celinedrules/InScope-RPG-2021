using System;
using Dialogue.GUI;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    public class ColorSelector : PopupWindowContent
    {
        private Texture2D aquaContent;
        private Texture2D blueContent;
        private Texture2D greyContent;
        private Texture2D greenContent;
        private Texture2D orangeContent;
        private Texture2D redContent;
        private Texture2D yellowContent;

        private readonly Dialogue dialogue;
        private readonly ColorType colorType;
        private GUIStyle style;

        private void SetColor(Texture2D newTex)
        {
            switch (colorType)
            {
                case ColorType.Player:
                    dialogue.PlayerColor = newTex;
                    break;
                case ColorType.Npc:
                    dialogue.NpcColor = newTex;
                    break;
                case ColorType.Connection:
                    dialogue.NodeConnectionColor = newTex;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null);
            }
        }
        
        public ColorSelector(Dialogue dialogue, ColorType colorType)
        {
            this.dialogue = dialogue;
            this.colorType = colorType;
        }
        

        public override Vector2 GetWindowSize()
        {
            return new Vector2(115, 225);
        }
        
        public override void OnOpen()
        {
            style = new GUIStyle()
            {
                margin = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(0,0,0,0)
            };

            aquaContent = DialogueEditorGUI.aquaButton;
            blueContent = DialogueEditorGUI.blueButton;
            greyContent = DialogueEditorGUI.greyButton;
            greenContent = DialogueEditorGUI.greenButton;
            orangeContent = DialogueEditorGUI.orangeButton;
            redContent = DialogueEditorGUI.redButton;
            yellowContent = DialogueEditorGUI.yellowButton;
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(DialogueEditorGUI.aquaButton, style, GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        SetColor(aquaContent);
                        editorWindow.Close();
                    }
                    else if (GUILayout.Button(blueContent, style, GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        SetColor(blueContent);
                        editorWindow.Close();
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(greyContent, style, GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        SetColor(greyContent);
                        editorWindow.Close();
                    }
                    else if (GUILayout.Button(greenContent, style, GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        SetColor(greenContent);
                        editorWindow.Close();
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(orangeContent, style, GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        SetColor(orangeContent);
                        editorWindow.Close();
                    }
                    else if (GUILayout.Button(redContent, style, GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        SetColor(redContent);
                        editorWindow.Close();
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(yellowContent, style, GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        SetColor(yellowContent);
                        editorWindow.Close();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}