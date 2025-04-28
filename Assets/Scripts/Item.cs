using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="Scriptable Object/Item")]
public class Item : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public string displayname;
    public Sprite sprite;
    public Sprite inventoryDisplay;
    public int id;
    public int sort;
    public int level;
    public Type type;
    public Category category;
    public CombatStyle combatStyle;
    public Rarity rarity;

    [Header("Stats")]
    public float damageMin;
    public float damageMax;
    public float attackSpeed;
    public float attackRange;
    public float health;
    public float mana;
    public float armor;
    public GameObject projectile;
    public float projectileSpeed;
    public float rarityDrop;

    [Header("Stats Bonus")]

    public List<float> attackSpeedBonus = new List<float>();
    public List<float> criticalChance = new List<float>();
    public List<float> criticalDamage = new List<float>();
    public List<float> spellDamage = new List<float>();
    public List<float> healthRegen = new List<float>();
    public List<float> manaRegen = new List<float>();
    public List<float> movementSpeed = new List<float>();
    public List<float> strength = new List<float>();
    public List<float> dexterity = new List<float>();
    public List<float> agility = new List<float>();
    public List<float> intelligence = new List<float>();

    [TextArea(10, 10)]
    public string description;

    [Header("Settings")]
    public Vector3 size;
    public Vector3 post;

    public Vector3 size2;
    public Vector3 post2;

    //public Vector3 testCopy;

    public bool resize;
    public bool repost;
    public bool idleAnimated;
    public bool stackable;

    public enum Category
    {
        SWORD,
        AXE,
        MACE,
        HAMMER,
        BOW,
        CROSSBOW,
        DAGGER,
        SCYTHE,
        SICKLE,
        STAFF,
        WAND,
        TOME,
        HELMET,
        CHESTPLATE,
        GREAVES,
        COIF,
        VEST,
        BOOTS,
        HOOD,
        CLOAK,
        SHOES,
        HAT,
        ROBE,
        SLIPPERS
    }

    public enum Type
    {
        Head,
        Body,
        Hand,
        Feet,
        Consumable,
        Miscellaneous
    }

    public enum CombatStyle
    {
        Melee,
        Range
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Immortal
    }
}
