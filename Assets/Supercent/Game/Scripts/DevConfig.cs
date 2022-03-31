using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EColor
{
    RED,
    BLUE,
    GREEN,
    YELLOW,
}

public static class ColorType
{
    public static Color GetColor(EColor eColor)
    {
        Color color;
        switch (eColor)
        {
            case EColor.RED:
                color = Color.red;
                break;
            case EColor.BLUE:
                color = Color.blue;
                break;
            case EColor.GREEN:
                color = Color.green;
                break;
            case EColor.YELLOW:
                color = Color.yellow;
                break;
            default:
                return Color.black;
                break;
        }

        color.a = 0.5f;
        return color;
    }
}
