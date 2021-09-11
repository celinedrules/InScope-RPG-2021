using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    public static class Grid
    {
        public static float GridSnapSize { get; set; } = 1;

        public static void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor, Vector2 canvasSize, Vector2 offset)
        {
            int widthDivs = Mathf.CeilToInt(canvasSize.x / gridSpacing);
            int heightDivs = Mathf.CeilToInt(canvasSize.y / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            //offset += draggingOffset * 0.5f;

            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                    new Vector3(gridSpacing * i, canvasSize.y, 0f) + newOffset);

            for (int j = 0; j < heightDivs; j++)
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                    new Vector3(canvasSize.x, gridSpacing * j, 0f) + newOffset);

            Handles.color = Color.white;
            Handles.EndGUI();
        }
    }
}