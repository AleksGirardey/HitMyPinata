using UnityEngine;

public struct HittingColor {
    public KeyCode KeyCode;
    public Color Color;

    public HittingColor(KeyCode keyCode, Color color) {
        KeyCode = keyCode;
        Color = color;
    }
}