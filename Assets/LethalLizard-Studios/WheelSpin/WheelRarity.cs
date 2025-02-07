using System.Collections.Generic;
using UnityEngine;

public enum WheelRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public static class WheelRarityExtensions
{
    private static readonly Dictionary<WheelRarity, Color> rarityColors = new Dictionary<WheelRarity, Color>
    {
        { WheelRarity.Common, new Color(0.607f, 0.564f, 0.631f) },
        { WheelRarity.Uncommon, new Color(0.031f, 0.521f, 0.925f) },
        { WheelRarity.Rare, new Color(0.521f, 0.031f, 0.925f) },
        { WheelRarity.Epic, new Color(0.925f, 0.031f, 0.604f) },
        { WheelRarity.Legendary, new Color(0.925f, 0.682f, 0.031f) }
    };

    public static Color GetColor(this WheelRarity rarity)
    {
        return rarityColors.TryGetValue(rarity, out Color color) ? color : Color.white;
    }
}
