using UnityEngine;

public static class UIUtils 
{
    private static Texture2D _rectTexture;

    public static Texture2D RectTexture
    {
        get
        {
            if (_rectTexture == null)
            {
                _rectTexture = new(1, 1);
                _rectTexture.SetPixel(0, 0, Color.white);
                _rectTexture.Apply();
            }
            return _rectTexture;
        }
    }

    public static void DrawSelectArea(Rect rect, Color areaColor, Color borderColor, float borderThickness = 1)
    {
        DrawRect(rect, areaColor);
        DrawBorder(rect, borderColor, borderThickness);
    }

    private static void DrawRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, RectTexture);
        GUI.color = Color.white;
    }
    private static void DrawBorder(Rect rect, Color color, float thickness)
    {
        DrawRect(new(rect.xMin, rect.yMin, rect.width, thickness), color);
        DrawRect(new(rect.xMin, rect.yMin, thickness, rect.height), color);
        DrawRect(new(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        DrawRect(new(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }
}
