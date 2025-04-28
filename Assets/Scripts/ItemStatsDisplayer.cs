using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;



public class ItemStatsDisplayer : MonoBehaviour
{
    [Header ("Reference")]
    public Inventory inventory;
    public GameObject itemDisplayObject;
    public bool itemDisplayObjectEnable;
    bool mouseHold;

    [Header ("Casual Display")]
    public TextMeshProUGUI displayname;
    public TextMeshProUGUI level;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI health;
    public TextMeshProUGUI type;
    public TextMeshProUGUI category;
    public TextMeshProUGUI attackSpeed;
    public TextMeshProUGUI dps;
    public TextMeshProUGUI armor;
    public List<TextMeshProUGUI> bonusStats = new List<TextMeshProUGUI>();
    public Image rarityBackground;
    public TextMeshProUGUI rarity;

    [Header("Color - Text (DisplayName)")]
    public Color commonDisplaynameColor;
    public Color uncommonDisplaynameColor;
    public Color rareDisplaynameColor;
    public Color epicDisplaynameColor;
    public Color legendaryDisplaynameColor;
    public Color immortalDisplaynameColor;

    [Header("Color - Text (Level)")]
    public Color commonLevelColor;
    public Color uncommonLevelColor;
    public Color rareLevelColor;
    public Color epicLevelColor;
    public Color legendaryLevelColor;
    public Color immortalLevelColor;

    [Header("Color - Rarity Background")]
    public Color commonBackgroundColor;
    public Color uncommonBackgroundColor;
    public Color rareBackgroundColor;
    public Color epicBackgroundColor;
    public Color legendaryBackgroundColor;
    public Color immortalBackgroundColor;

    [Header("Color - Text (Rarity)")]
    public Color commonRarityColor;
    public Color uncommonRarityColor;
    public Color rareRarityColor;
    public Color epicRarityColor;
    public Color legendaryRarityColor;
    public Color immortalRarityColor;

    private void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            bonusStats[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        itemDisplayObject.SetActive(itemDisplayObjectEnable);
    }

    public void CloseItemStatsDisplay()
    {
        itemDisplayObjectEnable = false;
    }

    public void CalculateStatsDisplay(Item theItem)
    {
        if (theItem.type == Item.Type.Hand)
        {
            damage.gameObject.SetActive(true);
            dps.gameObject.SetActive(true);
            attackSpeed.gameObject.SetActive(true);

            health.gameObject.SetActive(false);
            armor.gameObject.SetActive(false);
        }

        if (theItem.type == Item.Type.Head ||
            theItem.type == Item.Type.Body ||
            theItem.type == Item.Type.Feet
            )
        {
            health.gameObject.SetActive(true);
            armor.gameObject.SetActive(true);

            damage.gameObject.SetActive(false);
            dps.gameObject.SetActive(false);
            attackSpeed.gameObject.SetActive(false);
        }

        displayname.text = "" + theItem.displayname;
        level.text = "Level " + theItem.level;
        damage.text = "" + (int)theItem.damageMin + "-" + (int)theItem.damageMax + " Damage";
        type.text = "" + theItem.type;
        category.text = "" + theItem.category;
        attackSpeed.text = "<" + (int)theItem.attackSpeed + " Attack Speed>";
        dps.text = "(" + ((theItem.damageMin + theItem.damageMax) / 2).ToString("F0") + " Average DPS)";

        health.text = (int)theItem.health + " Health";
        armor.text = (int)theItem.armor + " Armor";

        for (int i = 0; i < theItem.strength.Count; i++)
        {
            if (!bonusStats[i].gameObject.activeSelf)
            {
                bonusStats[i].gameObject.SetActive(true);
                bonusStats[i].text = "+" + theItem.strength[i] + " Strength";
            }
        }

        for (int i = 0; i < theItem.dexterity.Count; i++)
        {
            if (!bonusStats[i].gameObject.activeSelf)
            {
                bonusStats[i].gameObject.SetActive(true);
                bonusStats[i].text = "+" + theItem.dexterity[i] + " Dexterity";
            }
        }

        for (int i = 0; i < theItem.agility.Count; i++)
        {
            if (!bonusStats[i].gameObject.activeSelf)
            {
                bonusStats[i].gameObject.SetActive(true);
                bonusStats[i].text = "+" + theItem.agility[i] + " Agility";
            }
        }

        for (int i = 0; i < theItem.intelligence.Count; i++)
        {
            if (!bonusStats[i].gameObject.activeSelf)
            {
                bonusStats[i].gameObject.SetActive(true);
                bonusStats[i].text = "+" + theItem.intelligence[i] + " Intelligence";
            }
        }

        for (int i = 0; i < theItem.criticalChance.Count; i++)
        {
            if (!bonusStats[i].gameObject.activeSelf)
            {
                bonusStats[i].gameObject.SetActive(true);
                bonusStats[i].text = "+" + theItem.criticalChance[i] + " Critical Chance";
            }
        }

        for (int i = 0; i < theItem.criticalChance.Count; i++)
        {
            if (!bonusStats[i].gameObject.activeSelf)
            {
                bonusStats[i].gameObject.SetActive(true);
                bonusStats[i].text = "+" + theItem.criticalChance[i] + " Critical Chance";
            }
        }

        for (int i = 0; i < theItem.attackSpeedBonus.Count; i++)
        {
            if (!bonusStats[i].gameObject.activeSelf)
            {
                bonusStats[i].gameObject.SetActive(true);
                bonusStats[i].text = "+" + theItem.attackSpeedBonus[i] + " Attack Speed";
            }
        }

        for (int i = 0; i < theItem.movementSpeed.Count; i++)
        {
            if (!bonusStats[i].gameObject.activeSelf)
            {
                bonusStats[i].gameObject.SetActive(true);
                bonusStats[i].text = "+" + theItem.movementSpeed[i] + " Movement Speed";
            }
        }

        for (int i = 0; i < theItem.spellDamage.Count; i++)
        {
            if (!bonusStats[i].gameObject.activeSelf)
            {
                bonusStats[i].gameObject.SetActive(true);
                bonusStats[i].text = "+" + theItem.spellDamage[i] + " Spell Damage";
            }
        }

        ChangeColor(theItem);

    }

    void ChangeColor(Item theItem)
    {
        if (theItem.rarity == Item.Rarity.Common)
        {
            displayname.color = new Color(commonDisplaynameColor.r, commonDisplaynameColor.g, commonDisplaynameColor.b);
            level.color = new Color(commonLevelColor.r, commonLevelColor.g, commonLevelColor.b);
            rarityBackground.color = new Color(commonBackgroundColor.r, commonBackgroundColor.g, commonBackgroundColor.b);
            rarity.color = new Color(commonRarityColor.r, commonRarityColor.g, commonRarityColor.b);
        }

        if (theItem.rarity == Item.Rarity.Uncommon)
        {
            displayname.color = new Color(uncommonDisplaynameColor.r, uncommonDisplaynameColor.g, uncommonDisplaynameColor.b);
            level.color = new Color(uncommonLevelColor.r, uncommonLevelColor.g, uncommonLevelColor.b);
            rarityBackground.color = new Color(uncommonBackgroundColor.r, uncommonBackgroundColor.g, uncommonBackgroundColor.b);
            rarity.color = new Color(uncommonRarityColor.r, uncommonRarityColor.g, uncommonRarityColor.b);
        }

        if (theItem.rarity == Item.Rarity.Rare)
        {
            displayname.color = new Color(rareDisplaynameColor.r, rareDisplaynameColor.g, rareDisplaynameColor.b);
            level.color = new Color(rareLevelColor.r, rareLevelColor.g, rareLevelColor.b);
            rarityBackground.color = new Color(rareBackgroundColor.r, rareBackgroundColor.g, rareBackgroundColor.b);
            rarity.color = new Color(rareRarityColor.r, rareRarityColor.g, rareRarityColor.b);
        }

        if (theItem.rarity == Item.Rarity.Epic)
        {
            displayname.color = new Color(epicDisplaynameColor.r, epicDisplaynameColor.g, epicDisplaynameColor.b);
            level.color = new Color(epicLevelColor.r, epicLevelColor.g, epicLevelColor.b);
            rarityBackground.color = new Color(epicBackgroundColor.r, epicBackgroundColor.g, epicBackgroundColor.b);
            rarity.color = new Color(epicRarityColor.r, epicRarityColor.g, epicRarityColor.b);
        }

        if (theItem.rarity == Item.Rarity.Legendary)
        {
            displayname.color = new Color(legendaryDisplaynameColor.r, legendaryDisplaynameColor.g, legendaryDisplaynameColor.b);
            level.color = new Color(legendaryLevelColor.r, legendaryLevelColor.g, legendaryLevelColor.b);
            rarityBackground.color = new Color(legendaryBackgroundColor.r, legendaryBackgroundColor.g, legendaryBackgroundColor.b);
            rarity.color = new Color(legendaryRarityColor.r, legendaryRarityColor.g, legendaryRarityColor.b);
        }

        if (theItem.rarity == Item.Rarity.Immortal)
        {
            displayname.color = new Color(immortalDisplaynameColor.r, immortalDisplaynameColor.g, immortalDisplaynameColor.b);
            level.color = new Color(immortalLevelColor.r, immortalLevelColor.g, immortalLevelColor.b);
            rarityBackground.color = new Color(immortalBackgroundColor.r, immortalBackgroundColor.g, immortalBackgroundColor.b);
            rarity.color = new Color(immortalRarityColor.r, immortalRarityColor.g, immortalRarityColor.b);
        }
    }
}
