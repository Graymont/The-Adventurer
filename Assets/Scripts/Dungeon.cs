using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="Scriptable Object/Dungeon")]
public class Dungeon : ScriptableObject
{
    public new string name;
    public string displayname;
    public Sprite sprite;
    public int level;
    public DungeonType dungeonType;
    public List<Npc> mob = new List<Npc>();
    public List<Item> rewards = new List<Item>();
    public List<int> rewardAmount = new List<int>();

    [Header("Raid Modifiers")]
    public int totalMob;
    public GameObject boss;

    public enum DungeonType
    {
        Regular,
        Raid
    }
}
