using UnityEngine;

public static class Theme
{
    public static readonly Color Background =
        Hex("#08121F");

    public static readonly Color Card =
        Hex("#172338");

    public static readonly Color CardHighlight =
        Hex("#22324D");

    public static readonly Color Primary =
        Hex("#2ECC71");

    public static readonly Color Gold =
        Hex("#F4C542");

    public static readonly Color Danger =
        Hex("#E74C3C");

    public static readonly Color Text =
        Color.white;

    public static readonly Color SecondaryText =
        Hex("#B6C4D8");

    static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out var color);
        return color;
    }
}