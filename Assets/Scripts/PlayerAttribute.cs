
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttribute : MonoBehaviour
{
    public PlayerController playerController;
    public SoundManager soundManager;
    public Chat chat;
    public List<GameObject> playerEquipmentList = new List<GameObject>();

    public Animator lowHealthAnimator;

    public float health;
    public float maxHealth;

    public float mana;
    public float maxMana;

    public float healthRegen;
    public float manaRegen;

    public float armor;

    public float strength;
    public float dexterity;
    public float agility;
    public float intelligence;

    public float spellDamage;
    public float movementSpeed;
    public float attackSpeed;
    public float criticalChance;
    public float criticalDamage;


    [Header ("Active Buff")]
    public List<BuffType> activeBuff = new List<BuffType>();
    public List<float> activeBuffDuration = new List<float>();
    [Header ("Buff UI")]
    public List<GameObject> abGameObject = new List<GameObject>();
    public List<Image> abImage = new List<Image>();
    public List<TextMeshProUGUI> abTextName = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> abTextDuration = new List<TextMeshProUGUI>();

    [Header ("Active Debuff")]
    public List<DebuffType> activeDebuff = new List<DebuffType>();
    public List<float> activeDebuffDuration = new List<float>();
    public List<float> activeDebuffDamage = new List<float>();
    [Header("Debuff UI")]
    public List<GameObject> adGameObject = new List<GameObject>();
    public List<Image> adImage = new List<Image>();
    public List<TextMeshProUGUI> adTextName = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> adTextDuration = new List<TextMeshProUGUI>();

    public bool stun;
    public bool silence;
    public bool blind;
    public bool disarm;
    public bool root;

    [Header ("Leveling")]
    public int shell;
    public int level;
    public float exp;
    public int[] maxexp = new int[100];
    public ClassList currentClass;

    public float money;

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider healthSliderTakeDamage;

    [SerializeField] Slider manaSlider;
    [SerializeField] Slider manaSliderTakeDamage;

    [SerializeField] Slider expSlider;
    [SerializeField] Slider expSliderTakeDamage;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] TextMeshProUGUI expTexts;

    public TextMeshProUGUI expText;
    public GameObject expTextGameObject;
    public float expTextTimer = 3;

    [Header("Multiplier")]
    public float movementSpeedMultiplier;
    public float jumpSpeedMultiplier;
    public float armorMultiplier;

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
        None,
        Stun,
        Slow,
        Silence,
        Blind,
        Root,
        Disarm,
        Poison
    }
    private void Awake()
    {
        health = maxHealth/2;
        soundManager = GetComponent<SoundManager>();
        chat = GetComponent<Chat>();
        __LevelInit__();
    }

    void __LevelInit__()
    {
        for (int i = 0; i < maxexp.Length; i++)
        {
            maxexp[i] = (50+i * 100) + (i * 2);
        }
    }

    [SerializeField] float everySecondTimer ;

    public void BuffModifier()
    {
        for (int i = 0; i < activeBuffDuration.Count; i++)
        {
            activeBuffDuration[i] -= Time.deltaTime;
        }

        for (int i = 0; i < activeBuff.Count; i++)
        {
            if (activeBuffDuration[i] <= 0)
            {
                activeBuff.RemoveAt(i);
                activeBuffDuration.RemoveAt(i); 
            }
        }
    }

    public void DebuffModifier()
    {
        for (int i = 0; i < activeDebuffDuration.Count; i++)
        {
            activeDebuffDuration[i] -= Time.deltaTime;
        }

        for (int i = 0; i < activeDebuff.Count; i++)
        {
            if (activeDebuffDuration[i] <= 0)
            {
                activeDebuff.RemoveAt(i);
                activeDebuffDuration.RemoveAt(i);
                activeDebuffDamage.RemoveAt(i);
            }
        }
    }
    public void DefaultAttribute()
    {
        float armorCalc = 5;
        for (int i = 0; i < playerEquipmentList.Count; i++)
        {
            if (playerEquipmentList[i].GetComponent<ItemHandler>().item != null)
            {
                armorCalc += playerEquipmentList[i].GetComponent<ItemHandler>().item.armor;
            }
        }
        armor = armorCalc*(1+armorMultiplier);
        armorCalc = 0;

        float maxHealthCalc = 500;
        for (int i = 0; i < playerEquipmentList.Count; i++)
        {
            if (playerEquipmentList[i].GetComponent<ItemHandler>().item != null)
            {
                maxHealthCalc += playerEquipmentList[i].GetComponent<ItemHandler>().item.health;
            }
        }
        maxHealth = maxHealthCalc;
        maxHealthCalc = 0;

        float maxManaCalc = 100;
        for (int i = 0; i < playerEquipmentList.Count; i++)
        {
            if (playerEquipmentList[i].GetComponent<ItemHandler>().item != null)
            {
                maxManaCalc += playerEquipmentList[i].GetComponent<ItemHandler>().item.mana;
            }
        }
        maxMana = maxManaCalc;
        maxManaCalc = 0;

        float strengthCalculate = 1;
        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.hand.GetComponent<ItemHandler>().item.strength)
            {
                strengthCalculate += stats;
            }
        }
        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.head.GetComponent<ItemHandler>().item.strength)
            {
                strengthCalculate += stats;
            }
        }
        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.body.GetComponent<ItemHandler>().item.strength)
            {
                strengthCalculate += stats;
            }
        }
        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.feet.GetComponent<ItemHandler>().item.strength)
            {
                strengthCalculate += stats;
            }
        }
        strength = strengthCalculate;

        float dexterityCalculate = 1;
        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.hand.GetComponent<ItemHandler>().item.dexterity)
            {
                dexterityCalculate += stats;
            }
        }
        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.head.GetComponent<ItemHandler>().item.dexterity)
            {
                dexterityCalculate += stats;
            }
        }
        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.body.GetComponent<ItemHandler>().item.dexterity)
            {
                dexterityCalculate += stats;
            }
        }
        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.feet.GetComponent<ItemHandler>().item.dexterity)
            {
                dexterityCalculate += stats;
            }
        }
        dexterity = dexterityCalculate;

        float agilityCalculate = 1;
        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.hand.GetComponent<ItemHandler>().item.agility)
            {
                agilityCalculate += stats;
            }
        }
        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.head.GetComponent<ItemHandler>().item.agility)
            {
                agilityCalculate += stats;
            }
        }
        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.body.GetComponent<ItemHandler>().item.agility)
            {
                agilityCalculate += stats;
            }
        }
        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.feet.GetComponent<ItemHandler>().item.agility)
            {
                agilityCalculate += stats;
            }
        }
        agility = agilityCalculate;

        float intelligenceCalculate = 1;
        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.hand.GetComponent<ItemHandler>().item.intelligence)
            {
                intelligenceCalculate += stats;
            }
        }
        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.head.GetComponent<ItemHandler>().item.intelligence)
            {
                intelligenceCalculate += stats;
            }
        }
        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.body.GetComponent<ItemHandler>().item.intelligence)
            {
                intelligenceCalculate += stats;
            }
        }
        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.feet.GetComponent<ItemHandler>().item.intelligence)
            {
                intelligenceCalculate += stats;
            }
        }
        intelligence = intelligenceCalculate;

        float criticalChanceCalculate = 5;
        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.hand.GetComponent<ItemHandler>().item.criticalChance)
            {
                criticalChanceCalculate += stats;
            }
        }
        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.head.GetComponent<ItemHandler>().item.criticalChance)
            {
                criticalChanceCalculate += stats;
            }
        }
        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.body.GetComponent<ItemHandler>().item.criticalChance)
            {
                criticalChanceCalculate += stats;
            }
        }
        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.feet.GetComponent<ItemHandler>().item.criticalChance)
            {
                criticalChanceCalculate += stats;
            }
        }
        criticalChance = criticalChanceCalculate;

        float criticalDamageCalculate = 105;
        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.hand.GetComponent<ItemHandler>().item.criticalDamage)
            {
                criticalDamageCalculate += stats;
            }
        }
        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.head.GetComponent<ItemHandler>().item.criticalDamage)
            {
                criticalDamageCalculate += stats;
            }
        }
        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.body.GetComponent<ItemHandler>().item.criticalDamage)
            {
                criticalDamageCalculate += stats;
            }
        }
        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.feet.GetComponent<ItemHandler>().item.criticalDamage)
            {
                criticalDamageCalculate += stats;
            }
        }
        criticalDamage = criticalDamageCalculate;

        float spellDamageCalculate = 10;
        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.hand.GetComponent<ItemHandler>().item.spellDamage)
            {
                spellDamageCalculate += stats;
            }
        }
        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.head.GetComponent<ItemHandler>().item.spellDamage)
            {
                spellDamageCalculate += stats;
            }
        }
        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.body.GetComponent<ItemHandler>().item.spellDamage)
            {
                spellDamageCalculate += stats;
            }
        }
        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.feet.GetComponent<ItemHandler>().item.spellDamage)
            {
                spellDamageCalculate += stats;
            }
        }
        spellDamage = spellDamageCalculate;

        float movementSpeedCalculate = 0;
        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.hand.GetComponent<ItemHandler>().item.movementSpeed)
            {
                movementSpeedCalculate += stats;
            }
        }
        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.head.GetComponent<ItemHandler>().item.movementSpeed)
            {
                movementSpeedCalculate += stats;
            }
        }
        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.body.GetComponent<ItemHandler>().item.movementSpeed)
            {
                movementSpeedCalculate += stats;
            }
        }
        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.feet.GetComponent<ItemHandler>().item.movementSpeed)
            {
                movementSpeedCalculate += stats;
            }
        }
        movementSpeed = movementSpeedCalculate*(1+movementSpeedMultiplier);

        float attackSpeedBonusCalculate = 0;
        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.hand.GetComponent<ItemHandler>().item.attackSpeedBonus)
            {
                attackSpeedBonusCalculate += stats;
            }
        }
        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.head.GetComponent<ItemHandler>().item.attackSpeedBonus)
            {
                attackSpeedBonusCalculate += stats;
            }
        }
        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.body.GetComponent<ItemHandler>().item.attackSpeedBonus)
            {
                attackSpeedBonusCalculate += stats;
            }
        }
        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            foreach (float stats in playerController.feet.GetComponent<ItemHandler>().item.attackSpeedBonus)
            {
                attackSpeedBonusCalculate += stats;
            }
        }
        attackSpeed = 125 - attackSpeedBonusCalculate;
    }

    public void LowHealth()
    {
        //Debug.Log(maxHealth / 5);
        lowHealthAnimator.SetBool("lowhealth", health <= maxHealth / 5);
    }

    public void DamagePlayer(float damage)
    {
        float damageCalculation = damage - armor;

        if (damageCalculation <= 0)
        {
            damageCalculation = 1;
        }

        health -= damageCalculation;
        soundManager.PlaySound("player_take_damage", 1, 1);
        Debug.Log("[PA] Damage by: " + damageCalculation +" Actual Damage: "+damage);
    }

    public void Heal(float amount)
    {
        health += amount;
        Debug.Log("[PA] Healed by: " + amount);
    }

    public void RestoreMana(int amount)
    {
        mana += amount;
        Debug.Log("[PA] Mana Restored by: " + amount);
    }

    public void EverySecond()
    {
        // Called Every Second 

        for (int i = 0; i < activeDebuff.Count; i++)
        {
            if (activeDebuff[i] == DebuffType.Poison)
            {
                DamagePlayer(activeDebuffDamage[i]);
            }
        }
    }

    private void Update()
    {
        everySecondTimer += Time.deltaTime;

        for (int i = 0; i < activeDebuff.Count; i++)
        {
            stun = false;
            silence = false;
            blind = false;
            root = false;

            if (activeDebuff[i] == DebuffType.Stun)
            {
                stun = true;
            }
            else if (activeDebuff[i] == DebuffType.Silence)
            {
                silence = true;
            }
            else if (activeDebuff[i] == DebuffType.Blind)
            {
                blind = true;
            }

            else if (activeDebuff[i] == DebuffType.Disarm)
            {
                disarm = true;
            }

            else if (activeDebuff[i] == DebuffType.Root)
            {
               root = true;
            }
        }

        if(everySecondTimer >= 1)
        {
            everySecondTimer = 0;
            EverySecond();
        }

        DefaultAttribute();
        BuffModifier();
        DebuffModifier();
        LowHealth();
        healthText.text = (int)health + "/" + (int)maxHealth;
        manaText.text = (int)mana + "/" + (int)maxMana;
        expTexts.text = (int)exp + "/" + (int)maxexp[level];

        if (health < 0)
        {
            KillPlayer();
        }

        else if (health > maxHealth)
        {
            health = maxHealth;
        }


        // Mana Regen
        else if (mana > maxMana)
        {
            mana = maxMana;
        }
        mana = Mathf.Max(mana, 0);
        //health = Mathf.Max(health, 0);

        // Health regen
        if (health < maxHealth)
        {
            health += (1 + healthRegen) * Time.deltaTime;
        }

        if (mana < maxMana)
        {
            mana += (1 + manaRegen) * Time.deltaTime;
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        healthSliderTakeDamage.maxValue = maxHealth;

        if (healthSliderTakeDamage.value > healthSlider.value)
        {
            healthSliderTakeDamage.value -= 50 * Time.deltaTime;
        }

        else if (healthSliderTakeDamage.value < healthSlider.value)
        {
            healthSliderTakeDamage.value = healthSlider.value;
        }

        manaSlider.maxValue = maxMana;
        manaSlider.value = mana;

        manaSliderTakeDamage.maxValue = maxMana;

        if (manaSliderTakeDamage.value > manaSlider.value)
        {
            manaSliderTakeDamage.value -= 50 * Time.deltaTime;
        }

        else if (manaSliderTakeDamage.value < manaSlider.value)
        {
            manaSliderTakeDamage.value = manaSlider.value;
        }

        expSlider.maxValue = maxexp[level];
        expSliderTakeDamage.value = exp;
        expSliderTakeDamage.maxValue = maxexp[level];

        if (expSlider.value < expSliderTakeDamage.value)
        {
            expSlider.value += 50 * Time.deltaTime;
        }

        else if (expSlider.value > expSliderTakeDamage.value)
        {
            expSlider.value = expSliderTakeDamage.value;
        }


        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < maxexp.Length; i++)
            {
                Debug.Log("Level "+i+" (" + maxexp[i]+")");
            }
        }

        if (exp > maxexp[level-1])
        {
            exp = 0;
            level++;

            soundManager.PlaySound("player_levelup", 1, 1);
            Debug.Log("[PA] Levelup!");
        }

        if (expTextTimer > 0)
        {
            expTextTimer -= Time.deltaTime;
            expTextGameObject.SetActive(true);
        }

        else if (expTextTimer <= 0)
        {
            expTextGameObject.SetActive(false);
        }

        expTextGameObject.transform.position = transform.position;

        // Buff UI
        for (int i = 0; i < abGameObject.Count; i++)
        {
            if (i >= activeBuff.Count)
            {
                abGameObject[i].SetActive(false);
            }

            else if (i < activeBuff.Count)
            {
                abGameObject[i].SetActive(true);
                abTextDuration[i].text = ""+(int)activeBuffDuration[i];

                if (activeBuff[i] == BuffType.Speed)
                {
                    abTextName[i].text = "Haste";
                }
                else if (activeBuff[i] == BuffType.Jump)
                {
                    abTextName[i].text = "Jump Boost";
                }
                else if (activeBuff[i] == BuffType.Armor)
                {
                    abTextName[i].text = "Protection";
                }
            }
        }

        // Debuff UI
        for (int i = 0; i < adGameObject.Count; i++)
        {
            if (i >= activeDebuff.Count)
            {
                adGameObject[i].SetActive(false);
            }

            else if (i < activeDebuff.Count)
            {
                adGameObject[i].SetActive(true);
                adTextDuration[i].text = "" + (int)activeDebuffDuration[i];
                adTextName[i].text = "" + activeDebuff[i];
            }
        }
    }

    public void KillPlayer()
    {
        GameObject spawnPoint = GameObject.Find("Spawn Point");
        Vector3 spawnLocation = spawnPoint.transform.position;
        transform.position = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);

        health = maxHealth;
        chat.AddText("Respawned!", Color.green);
    }

    public void AddExp(float amount)
    {
        exp += amount;
        Debug.Log("EXP: +" + amount);
    }

    public enum ClassList
    {
        Warrior,
        Archer,
        Assassin,
        Mage,
    }
}
