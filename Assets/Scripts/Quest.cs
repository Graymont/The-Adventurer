using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="Scriptable Object/Quest")]
public class Quest : ScriptableObject
{
    [Header ("Essentials")]
    public new string name;
    public string displayname;
    public int id;
    public int level;
    public int amount = 1;

    [Header("Criteria")]
    public string targetName;
    public string itemName;
    public bool repeatable;
    public Type type;
    public Item deliveryItem;

    [Header("Reward")]
    public List<Item> rewards = new List<Item>();
    public List<int> rewardAmount = new List<int>();
    public float expReward;
    public int shellReward;

    [TextArea(10, 10)]
    public string story;

    [TextArea(10, 10)]
    public string description;

    public enum Type
    {
        Kill,
        Delivery,
        Interaction
    }
}
