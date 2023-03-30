using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create a rarity enum that will be use on items
/// </summary>
public enum ERarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

/// <summary>
/// Rarity object containing all informations about rarity : name, color.
/// </summary>
public class Rarity
{
    public string Name;
    public Color Color;

    public Rarity(string name, Color color)
    {
        Name = name;
        Color = color;
    }
}

/// <summary>
/// Rarity manager containing all rarity objects, call this class with enum to get informations about specific rarity.
/// </summary>
public static class RarityManager
{
    private static Dictionary<ERarity, Rarity> Rareties = new Dictionary<ERarity, Rarity>() 
    {
        { ERarity.Common, new Rarity("Common", new Color(1f, 1f, 1f, 1f)) },
        { ERarity.Uncommon, new Rarity("Uncommon", new Color(0.12f, 1f, 0f, 1f)) },
        { ERarity.Rare, new Rarity("Rare", new Color(0f, 0.44f, 0.87f, 1f)) },
        { ERarity.Epic, new Rarity("Epic", new Color(0.64f, 0.21f, 0.93f, 1f)) },
        { ERarity.Legendary, new Rarity("Legendary", new Color(1f, 0.5f, 0f, 1f)) }
    };

    public static Color GetRarityColor(ERarity itemRarity)
    {
        return Rareties[itemRarity].Color;
    }

    public static string GetRarityName(ERarity itemRarity)
    {
        return Rareties[itemRarity].Name;
    }

    public static Rarity GetRarity(ERarity itemRarity) 
    {
        return Rareties[itemRarity];
    }
}