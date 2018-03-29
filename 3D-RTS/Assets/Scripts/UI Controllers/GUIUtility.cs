using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *  Reference:
 *  SelectionRect script from http://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/ 
 */
public static class GUIUtility {

    static Texture2D selectionRect;
    public static Texture2D SelectionRect
    {

        get
        {
            if (selectionRect == null)
            {
                selectionRect = new Texture2D(1, 1);
                selectionRect.SetPixel(0, 0, Color.white);
                selectionRect.Apply();
            }

            return selectionRect;
        }

    }

    public static void DrawSelectionRect(Rect rect, Color color)
    {
        GUI.depth = 0;
        GUI.color = color;
        GUI.DrawTexture(rect, SelectionRect);
        GUI.color = Color.white;
    }

    public static void DrawSelectionRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        GUIUtility.DrawSelectionRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        GUIUtility.DrawSelectionRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        GUIUtility.DrawSelectionRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        GUIUtility.DrawSelectionRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

}
