using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="Scriptable Object/Skill")]
public class Skill : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public int id;
    public string displayname;
    public Sprite sprite;
    public int level;
    public SkillType skillType;
    public CastType castType;
    public GameObject projectile;

    [Header("Attributes")]
    public int cooldown;
    public float damage;
    public float healAmount;
    public float dashRange;
    public float blinkRange;
    public float castRange;
    public float aoeRange;
    public float manaCost;
    public float healthCost;
    public float castAmount;
    public float castDelayInBetween;
    public float castChannelingTime;

    [Header ("Buff")]
    public BuffType buffType;
    public float buffAmplifier;
    public float buffDuration;

    [Header("Debuff")]
    public DebuffType debuffType;
    public float debuffDamage;
    public float debuffDuration;

    [Header("Modifiers")]
    public bool multiple;
    public bool target;

    public List<AnimationClip> animations = new List<AnimationClip>();
    public string soundEffect;
    public string triggerAnimation;
    public enum SkillType
    {
        Dash,
        Heal,
        Blink,
        Buff,
        Debuff,
        Damage,
        CC,
        Projectile
    }

    public enum CastType
    {
        TargetLock,
        FreeCast
    }

    public enum BuffType
    {
        None,
        Speed,
        Jump,
        Armor,
        Invisibility,
        Regeneration,
        Clarity,
        Strength,
        Dexterity,
        Intelligence,
        Agility
    }

    public enum DebuffType
    {
        Stun,
        Slow,
        Silence,
        Blind,
        Root,
        Disarm,
        Poison
    }
}
