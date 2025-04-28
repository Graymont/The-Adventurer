using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillHandler : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public AudioSource audios;
    public Chat chat;
    public PlayerAttribute playerAttribute;
    public PlayerController playerController;
    public SoundManager soundManager;

    [SerializeField] Vector3 skillMousePosition;
    [SerializeField] float skillDistanceBetweenPlayer;

    [Header("Modif")]
    public List<Skill> sSlot = new List<Skill>();
    public List<float> sCooldown = new List<float>();
    public List<float> sCastingAmount = new List<float>();
    public List<float> sCastingDelayTimer = new List<float>();
    public List<float> sCastingDelayInBetween = new List<float>();
    public List<TextMeshProUGUI> sText = new List<TextMeshProUGUI>();
    public List<Skill> sCastingSkill = new List<Skill>();
    public List<float> sCastingChanneling = new List<float>();
    public List<bool> sCastingChannelingBool = new List<bool>();
    public List<Image> sImage = new List<Image>();

    public Sprite emptySkillSprite;

    public LayerMask enemyLayer;

    public float upwards;
    public float forwards;
    public float backwards;

    [Header("Channeling UI")]
    public GameObject channelingGameObjectUI;
    public Slider channelingSlider;
    public TextMeshProUGUI channelingDurationText;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audios = GetComponent<AudioSource>();
        soundManager = GetComponent<SoundManager>();
    }

    public void OnSkillButtonPressed(int number)
    {
        if (sSlot[number] != null)
        {
            CastSkill(sSlot[number]);
        }
    }

    private void Update()
    {
        SkillBind();
        skillMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        skillDistanceBetweenPlayer = Vector2.Distance(skillMousePosition, transform.position);

        // Skill Cooldown
        for (int i = 0; i < sCooldown.Count; i++)
        {
            if (sCooldown[i] > 0)
            {
                sCooldown[i] -= Time.deltaTime;
            }

            else if (sCooldown[i] < 0)
            {
                sCooldown[i] = 0;
            }
        }

        // Skill Slot
        for (int i = 0; i < sSlot.Count; i++)
        {
            if (sSlot[i] == null)
            {
                sText[i].gameObject.SetActive(false);
                sImage[i].GetComponent<Image>().sprite = emptySkillSprite;
            }

            else if (sSlot[i] != null && sCooldown[sSlot[i].id] > 0)
            {
                sText[i].gameObject.SetActive(true);
                sText[i].text = "" + (int)sCooldown[sSlot[i].id];
                sImage[i].GetComponent<Image>().sprite = sSlot[i].sprite;

                Color cooldownColor = sImage[i].GetComponent<Image>().color;
                cooldownColor.a = 125f / 255f;
                sImage[i].GetComponent<Image>().color = cooldownColor;
            }

            else if (sSlot[i] != null && sCooldown[sSlot[i].id] == 0)
            {
                sText[i].gameObject.SetActive(false);
                sImage[i].GetComponent<Image>().sprite = sSlot[i].sprite;

                Color cooldownColor = sImage[i].GetComponent<Image>().color;
                cooldownColor.a = 1;
                sImage[i].GetComponent<Image>().color = cooldownColor;
            }
        }

        // Between delay in Multicast
        for (int i = 0; i < sCastingDelayTimer.Count; i++)
        {
            sCastingDelayTimer[i] += Time.deltaTime;
        }

        // Multicast
        for (int i = 0; i < sCastingAmount.Count; i++)
        {
            if (sCastingAmount[i] > 0 && sCastingDelayTimer[i] >= sCastingDelayInBetween[i])
            {
                sCastingDelayTimer[i] = 0;
                sCastingAmount[i]--;

                if (sCastingSkill[i].skillType == Skill.SkillType.Damage)
                {
                    DamageArea(sCastingSkill[i]);
                }
                else if (sCastingSkill[i].skillType == Skill.SkillType.Buff)
                {
                    BuffPlayer("" + sCastingSkill[i].buffType, sCastingSkill[i].buffAmplifier, sCastingSkill[i].buffDuration);
                }
                else if (sCastingSkill[i].skillType == Skill.SkillType.Debuff)
                {
                    DebuffPlayer("" + sCastingSkill[i].debuffType, sCastingSkill[i].debuffDamage, sCastingSkill[i].debuffDuration);
                }
                else if (sCastingSkill[i].skillType == Skill.SkillType.Blink)
                {
                    // Blink
                }
                else if (sCastingSkill[i].skillType == Skill.SkillType.Projectile)
                {
                    ShootProjectile(sCastingSkill[i]);
                }
                else if (sCastingSkill[i].skillType == Skill.SkillType.Dash)
                {
                    PushPlayerForwards(sCastingSkill[i].dashRange);
                }
                else if (sCastingSkill[i].skillType == Skill.SkillType.CC)
                {
                    AreaOfEffect(sCastingSkill[i]);
                }

            }
        }

        // Channeling
        for (int i = 0; i < sCastingChanneling.Count; i++)
        {
            if (sCastingSkill[i] != null && sCastingChanneling[i] <= sCastingSkill[i].castChannelingTime && sCastingChannelingBool[i])
            {
                sCastingChanneling[i] += Time.deltaTime;

                channelingDurationText.text = (int)sCastingChanneling[i]+"/"+ sCastingSkill[i].castChannelingTime+"s";
                channelingSlider.value = sCastingChanneling[i];
                channelingSlider.maxValue = sCastingSkill[i].castChannelingTime;

                if (sCastingChanneling[i] >= sCastingSkill[i].castChannelingTime)
                {
                    sCastingChannelingBool[i] = false;
                    sCastingChanneling[i] = 0;

                    if (sCastingSkill[i].multiple)
                    {
                        MultipleCast(sCastingSkill[i]);
                    }

                    else if (!sCastingSkill[i].multiple)
                    {
                        if (sCastingSkill[i].skillType == Skill.SkillType.Damage)
                        {
                            DamageArea(sCastingSkill[i]);
                        }
                        else if (sCastingSkill[i].skillType == Skill.SkillType.Buff)
                        {
                            BuffPlayer("" + sCastingSkill[i].buffType, sCastingSkill[i].buffAmplifier, sCastingSkill[i].buffDuration);
                        }
                        else if (sCastingSkill[i].skillType == Skill.SkillType.Blink)
                        {
                            // Blink
                        }
                        else if (sCastingSkill[i].skillType == Skill.SkillType.Projectile)
                        {
                            ShootProjectile(sCastingSkill[i]);
                        }
                        else if (sCastingSkill[i].skillType == Skill.SkillType.Dash)
                        {
                            PushPlayerForwards(sCastingSkill[i].dashRange);
                        }
                        else if (sCastingSkill[i].skillType == Skill.SkillType.CC)
                        {
                            AreaOfEffect(sCastingSkill[i]);
                        }
                    }

                    channelingGameObjectUI.SetActive(false);
                }
            }
        }

        
    }

    void SkillBind()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (sSlot[0] != null)
            {
                CastSkill(sSlot[0]);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (sSlot[1] != null)
            {
                CastSkill(sSlot[1]);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (sSlot[2] != null)
            {
                CastSkill(sSlot[2]);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (sSlot[3] != null)
            {
                CastSkill(sSlot[3]);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (sSlot[4] != null)
            {
                CastSkill(sSlot[4]);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (sSlot[5] != null)
            {
                CastSkill(sSlot[5]);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (sSlot[6] != null)
            {
                CastSkill(sSlot[6]);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (sSlot[7] != null)
            {
                CastSkill(sSlot[7]);
            }
        }
    }

    void PushPlayerModifier()
    {
        float directionForward = 0;
        float directionBackward = 0;

        if (transform.localScale.x > 0)
        {
            directionForward = 1;
            directionBackward = -1;
        }

        else if (transform.localScale.x < 0)
        {
            directionForward = -1;
            directionBackward = 1;
        }

        if (upwards > 0)
        {
            upwards -= 25 * Time.deltaTime;
            rb.AddForce(transform.up * 10, ForceMode2D.Impulse);
        }

        if (forwards > 0)
        {
            forwards -= 25 * Time.deltaTime;
            rb.AddForce(directionForward * Vector2.right * 300, ForceMode2D.Impulse);
        }

        if (backwards > 0)
        {
            backwards -= 25 * Time.deltaTime;
            rb.AddForce(directionBackward * Vector2.right * 300, ForceMode2D.Impulse);
        }

        upwards = Mathf.Max(upwards, 0);
        forwards = Mathf.Max(forwards, 0);
        backwards = Mathf.Max(backwards, 0);
    }

    private void FixedUpdate()
    {
        PushPlayerModifier();
    }

    public void PushPlayerUpwards(float amplifier)
    {
        upwards = amplifier;
    }

    public void PushPlayerForwards(float amplifier)
    {
        forwards = amplifier;
    }

    public void PushPlayerBackwards(float amplifier)
    {
        backwards = amplifier;
    }

    public void DamageArea(Skill skill)
    {
        Collider2D[] castCircle = Physics2D.OverlapCircleAll(transform.position, skill.aoeRange, enemyLayer);

        if (!skill.target)
        {
            foreach (Collider2D e in castCircle)
            {
                e.GetComponent<NpcHandler>().SetAttacker(gameObject);
                e.GetComponent<NpcHandler>().DamageNpc(skill.damage, "magical");
            }
        }

        else if (skill.target)
        {
            if (skillDistanceBetweenPlayer <= skill.castRange)
            {
                Collider2D[] castCircle2 = Physics2D.OverlapCircleAll(skillMousePosition, skill.aoeRange, enemyLayer);

                GameObject targetNpc = null;
                foreach (Collider2D e in castCircle2)
                {
                    float distanceFromEnemy = Vector2.Distance(e.transform.position, skillMousePosition);

                    if (distanceFromEnemy <= 5)
                    {
                        targetNpc = e.gameObject;
                        break;
                    }
                }

                if (targetNpc != null)
                {
                    Collider2D[] aoeDamageTarget = Physics2D.OverlapCircleAll(targetNpc.transform.position, skill.aoeRange, enemyLayer);

                    foreach (Collider2D entity in aoeDamageTarget)
                    {
                        entity.GetComponent<NpcHandler>().SetAttacker(gameObject);
                        entity.GetComponent<NpcHandler>().DamageNpc(skill.damage, "magical");
                    }
                    targetNpc.GetComponent<NpcHandler>().SetAttacker(gameObject);
                    targetNpc.GetComponent<NpcHandler>().DamageNpc(skill.damage, "magical");
                }

                else if (targetNpc == null)
                {
                    chat.AddText("Target Not Found!", Color.red);
                }
            }
            else if (skillDistanceBetweenPlayer > skill.castRange)
            {
                chat.AddText("Target is too Far!", Color.red);
            }
        }
    }


    public void AreaOfEffect(Skill skill)
    {
        Collider2D[] castCircle = Physics2D.OverlapCircleAll(transform.position, skill.aoeRange, enemyLayer);

        if (!skill.target)
        {
            foreach (Collider2D e in castCircle)
            {
                e.GetComponent<NpcHandler>().SetAttacker(gameObject);
                e.GetComponent<NpcHandler>().DebuffNpc("" + skill.debuffType, skill.debuffDamage, skill.debuffDuration);
            }
        }

        else if (skill.target)
        {
            if (skillDistanceBetweenPlayer <= skill.castRange)
            {
                Collider2D[] castCircle2 = Physics2D.OverlapCircleAll(skillMousePosition, skill.aoeRange, enemyLayer);

                GameObject targetNpc = null;
                foreach (Collider2D e in castCircle2)
                {
                    float distanceFromEnemy = Vector2.Distance(e.transform.position, skillMousePosition);

                    if (distanceFromEnemy <= 5)
                    {
                        targetNpc = e.gameObject;
                        break;
                    }
                }

                if (targetNpc != null)
                {
                    Collider2D[] aoeDamageTarget = Physics2D.OverlapCircleAll(targetNpc.transform.position, skill.aoeRange, enemyLayer);

                    foreach (Collider2D entity in aoeDamageTarget)
                    {
                        entity.GetComponent<NpcHandler>().SetAttacker(gameObject);
                        entity.GetComponent<NpcHandler>().DebuffNpc("" + skill.debuffType, skill.debuffDamage, skill.debuffDuration);
                    }
                    targetNpc.GetComponent<NpcHandler>().SetAttacker(gameObject);
                    targetNpc.GetComponent<NpcHandler>().DebuffNpc("" + skill.debuffType, skill.debuffDamage, skill.debuffDuration);
                }

                else if (targetNpc == null)
                {
                    chat.AddText("Target Not Found!", Color.red);
                }
            }
            else if (skillDistanceBetweenPlayer > skill.castRange)
            {
                chat.AddText("Target is too Far!", Color.red);
            }
        }
    }


    public void Heal(Skill skill)
    {
        playerAttribute.Heal(skill.healAmount);
    }

    public void RestoreMana(int amount)
    {
        playerAttribute.RestoreMana(amount);
    }

    public void BuffPlayer(string name, float amplifier, float duration)
    {
        PlayerAttribute.BuffType bType = PlayerAttribute.BuffType.None;

        if (name == "Speed")
        {
            bType = PlayerAttribute.BuffType.Speed;
            playerAttribute.movementSpeedMultiplier = amplifier;
        }

        else if (name == "Jump")
        {
            bType = PlayerAttribute.BuffType.Jump;
            playerAttribute.jumpSpeedMultiplier = amplifier;
        }

        else if (name == "Armor")
        {
            bType = PlayerAttribute.BuffType.Armor;
            playerAttribute.armorMultiplier = amplifier;
        }

        playerAttribute.activeBuff.Add(bType);
        playerAttribute.activeBuffDuration.Add(duration);
    }

    public void DebuffPlayer(string name, float amount, float duration)
    {
        PlayerAttribute.DebuffType bType = PlayerAttribute.DebuffType.None;

        if (name == "Stun")
        {
            bType = PlayerAttribute.DebuffType.Stun;
        }

        else if (name == "Slow")
        {
            bType = PlayerAttribute.DebuffType.Slow;
        }

        else if (name == "Silence")
        {
            bType = PlayerAttribute.DebuffType.Silence;
        }

        else if (name == "Blind")
        {
            bType = PlayerAttribute.DebuffType.Blind;
        }

        else if (name == "Root")
        {
            bType = PlayerAttribute.DebuffType.Root;
        }

        else if (name == "Poison")
        {
            bType = PlayerAttribute.DebuffType.Poison;
        }

        else if (name == "Disarm")
        {
            bType = PlayerAttribute.DebuffType.Disarm;
        }

        playerAttribute.activeDebuff.Add(bType);
        playerAttribute.activeDebuffDuration.Add(duration);
        playerAttribute.activeDebuffDamage.Add(duration);
    }


    public void ShootProjectile(Skill skill)
    {
        anim.SetTrigger("Range");

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 attackPointPosition = playerController.hand.GetComponent<ItemHandler>().attackPoint_range.position;
        Vector3 direction = (mousePosition - attackPointPosition).normalized;

        GameObject projectile = Instantiate(skill.projectile, attackPointPosition, Quaternion.identity);
        projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        projectile.GetComponent<ProjectileHandler>().target = direction;
        projectile.GetComponent<ProjectileHandler>().ProjectileTimer(playerAttribute.level+3);
        projectile.GetComponent<ProjectileHandler>().speed = playerAttribute.level+55;
        float damageRandom = UnityEngine.Random.Range(playerController.hand.GetComponent<ItemHandler>().item.damageMin, playerController.hand.GetComponent<ItemHandler>().item.damageMax);
        float damage = damageRandom + playerAttribute.dexterity;

        projectile.GetComponent<ProjectileHandler>().Damage(damage);
        projectile.GetComponent<ProjectileHandler>().SetAttacker(gameObject);
    }

    // Skill


    public void CastSkill(Skill skill)
    {
        if (sCooldown[skill.id] <= 0)
        {
            sCooldown[skill.id] = skill.cooldown;
            soundManager.PlaySound("player_skill_"+skill.name, 1, 1);
            anim.SetTrigger("skill_" + skill.triggerAnimation);

            if (skill.name == "slash")
            {
                DamageArea(skill);
            }

            else if (skill.name == "reinforcedarmor")
            {
                BuffPlayer("" + skill.buffType, skill.buffAmplifier, skill.buffDuration);
            }

            else if (skill.name == "rage")
            {
                ChannelingCast(skill);
            }

            else if (skill.name == "focusburst")
            {
                ChannelingCast(skill);
            }

            else if (skill.name == "frenzy")
            {
                BuffPlayer(""+skill.buffType, skill.buffAmplifier, skill.buffDuration);
            }

            else if (skill.name == "spread")
            {
                ChannelingCast(skill);
            }
        }

        else if (sCooldown[skill.id] > 0) 
        {
            chat.AddText("Skill is on Cooldown!", Color.red);
        }
    }

    public void ChannelingCast(Skill skill)
    {
        sCastingSkill[skill.id] = skill;
        sCastingChanneling[skill.id] = 0;
        sCastingChannelingBool[skill.id] = true;

        channelingGameObjectUI.SetActive(true);
    }

    public void MultipleCast(Skill skill)
    {
        sCastingSkill[skill.id] = skill;
        sCastingAmount[skill.id] = skill.castAmount;
        sCastingDelayInBetween[skill.id] = skill.castDelayInBetween;
    }


    /*
     * 
     * Warrior - Slash, Reinforced Armor, Rage, Dragon Blood, Laser Beam
     * Archer - Focus Burst, Frenzy, Spread, Arrow Rain, Nature Shot
     * Assassin - Smoke Attack, Vanish, Shadow Dash, Instant Travel, Mark
     * Mage - Sacred Totem, Remedy, Magic Blast, Levitation, Ice Spike
     */
}
