using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Npc")]
public class Npc : ScriptableObject
{
    [Header ("Info")]
    public new string name;
    public string displayname;
    public int level;
    public Sprite potrait;
    public Sprite deathBodySprite;
    public GameObject deathBodyObject;
    public GameObject projectile;
    public Type type;
    public Category category;
    public Interaction interaction;
    public Quest quest;

    [Header ("Stats")]
    public float health;
    public float armor;
    public float damageMin;
    public float damageMax;
    public float attackSpeed;
    public float projectileSpeed;
    public float attackRange;
    public float movementSpeed;
    public float jumpSpeed;
    public float healthRegen;

    [Header ("AI Pathfind")]
    public float followRange;
    public float closestTargetRange;

    public float maxRight;
    public float maxLeft;
    public float changeBehaviourCooldown;
    public float changePathfindCooldown;

    [Header ("Dialog")]
    [TextArea(10, 10)] public List<string> dialog = new List<string>();

    [Header("Animation")]
    public string idle;
    public string hurt;
    public string walk;
    public string melee;
    public string range;

    [Header("Drops")]
    public bool lootSystem;
    public bool instantDrop;
    public List<Item> drops = new List<Item>();

    public enum Type
    {
        Melee,
        Range
    } 

    public enum Category
    {
        Allies,
        Enemy,
        Animal,
        TownNpc
    }

    public enum Interaction
    {
        Merchant,
        Quest,
        Dialogue
    }
}
