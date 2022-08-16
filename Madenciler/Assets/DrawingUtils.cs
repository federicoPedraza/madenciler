using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingUtils
{
    public static Cast[] castColors = new Cast[]
    {
        new Cast(CastMove.RIGHT, new Color32(254, 95, 85, 255), new float[] { 0.999f, 0.372f, 0.333f }, "#FE5F55", SpellElements.FIRE, "V"),
        new Cast(CastMove.DOWN, new Color32(85, 193, 255, 255), new float[] { 0.372f, 0.756f, 1f }, "#55C1FF", SpellElements.WATER, "K"),
        new Cast(CastMove.LEFT, new Color32(144, 252, 249, 255), new float[] { 0.564f, 0.988f, 0.976f }, "#90FCF9", SpellElements.AIR, "M"),
        new Cast(CastMove.UP, new Color32(205, 231, 176, 255), new float[] { 0.804f, 0.905f, 0.690f }, "#CDE7B0", SpellElements.EARTH, "O"),
        new Cast(CastMove.NONE, new Color32(244, 247, 245, 255), new float[] { 0.956f, 0.968f, 0.961f }, "#FFFFFF",  SpellElements.NONE, "1"),
    };

    public static Color Color32ToColor(Color32 color)
    {
        return new Color(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
    }

    public static Color GetElementColor(SpellElements element)
    {
        switch (element)
        {
            case SpellElements.FIRE:
                return castColors[0].color;
            case SpellElements.WATER:
                return castColors[1].color;
            case SpellElements.AIR:
                return castColors[2].color;
            case SpellElements.EARTH:
                return castColors[3].color;
            default:
                return castColors[4].color;
        }
    }

    public static float[] GetElementColorRGB(SpellElements element)
    {
        switch (element)
        {
            case SpellElements.FIRE:
                return castColors[0].colorRGB;
            case SpellElements.WATER:
                return castColors[1].colorRGB;
            case SpellElements.AIR:
                return castColors[2].colorRGB;
            case SpellElements.EARTH:
                return castColors[3].colorRGB;
            default:
                return castColors[4].colorRGB;
        }
    }

    public static string GetInTextElementColor(CastMove move)
    {
        switch (move)
        {
            case CastMove.RIGHT:
                return castColors[0].colorHEX;
            case CastMove.DOWN:
                return castColors[1].colorHEX;
            case CastMove.LEFT:
                return castColors[2].colorHEX;
            case CastMove.UP:
                return castColors[3].colorHEX;
            default:
                return castColors[4].colorHEX;
        }
    }

    public static SpellElements GetSpellElement(CastMove move)
    {
        switch (move)
        {
            case CastMove.RIGHT:
                return castColors[0].element;
            case CastMove.DOWN:
                return castColors[1].element;
            case CastMove.LEFT:
                return castColors[2].element;
            case CastMove.UP:
                return castColors[3].element;
            default:
                return castColors[4].element;
        }

    }

    public static string ElementToString(SpellElements element)
    {
        switch (element)
        {
            case SpellElements.FIRE:
                return castColors[0].elementText;
            case SpellElements.WATER:
                return castColors[1].elementText;
            case SpellElements.AIR:
                return castColors[2].elementText;
            case SpellElements.EARTH:
                return castColors[3].elementText;
            default:
                return castColors[4].elementText;
        }
    }

    public static string CastMoveToString(CastMove move)
    {
        switch (move)
        {
            case CastMove.DOWN:
                return castColors[0].elementText;
            case CastMove.UP:
                return castColors[1].elementText;
            case CastMove.RIGHT:
                return castColors[2].elementText;
            case CastMove.LEFT:
                return castColors[3].elementText;
            default:
                return castColors[4].elementText;
        }
    }
}

public class Cast
{
    public CastMove move;
    public Color32 color;
    public float[] colorRGB;
    public string colorHEX;
    public SpellElements element;
    public string elementText;

    public Cast(CastMove move, Color32 color,float[] colorRGB, string colorHEX, SpellElements element, string elementText)
    {
        this.move = move;
        this.color = color;
        this.colorRGB = colorRGB;
        this.colorHEX = colorHEX;
        this.element = element;
        this.elementText = elementText;
    }
}
