
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcHandler : MonoBehaviour
{
    [Header ("Core")]
    public GameObject player;
    public GameObject enemy;
    public GameObject allies;

    [Header ("Attacker")]
    public GameObject attacker;

    [Header("Rest")]
    [SerializeField] GameObject spawnerList;

    [Header ("Atts for Spawner")]
    public int MOB_SPAWNER_ID;

    [Header ("UI")]
    [SerializeField] MobDisplayHandler mobDisplayHandler;

    [Header ("Components")]
    public Npc npc;
    public Chat chat;
    public Rigidbody2D rb;
    public Animator anim;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask alliesLayer;

    [Header ("Modifiers")]
    public bool isTarget;
    public bool isFollow;
    public bool agroPlayer;
    public bool agroAllies;
    public float health;
    public float maxHealth;
    public float attackTimer;
    public float jumpTimer;
    public bool canUp;
    public bool collisionBlocking;
    public bool cliffHangerBlocking;
    public bool canDown;
    public float rightAmount;
    public float leftAmount;
    public bool right;
    public bool left;
    [SerializeField] float fixMovementTimer;

    [Header ("AI Collision Stuff")]
    Vector2 bodyScale;
    public GameObject upCheck1;
    public GameObject upCheck2;
    public GameObject downCheck1;
    public GameObject downCheck2;
    public GameObject blockingCheck1;
    public GameObject blockingCheck2;
    public GameObject cliffhangerCheck1;
    public GameObject cliffhangerCheck2;

    [Header ("Rewards")]
    public List<Item> dropList = new List<Item>();

    public List<BuffType> activeBuff = new List<BuffType>();
    public List<float> activeBuffDuration = new List<float>();

    public List<DebuffType> activeDebuff = new List<DebuffType>();
    public List<float> activeDebuffDuration = new List<float>();
    public List<float> activeDebuffDamage = new List<float>();

    public bool stun;
    public bool silence;
    public bool blind;
    public bool disarm;
    public bool root;

    public WorldSettings worldSettings;

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
        maxHealth = npc.health;
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        bodyScale = transform.localScale;
        right = true;
        left = false;
        dropList = npc.drops;

        rightRandom = 10;
        leftRandom = 10;
    }

    void RightCollisionDetector()
    {
        upCheck2.SetActive(true);
        downCheck2.SetActive(true);
        blockingCheck2.SetActive(true);
        cliffhangerCheck2.SetActive(true);

        upCheck1.SetActive(false);
        downCheck1.SetActive(false);
        blockingCheck1.SetActive(false);
        cliffhangerCheck1.SetActive(false);
    }

    void LeftCollisionDetector()
    {
        upCheck1.SetActive(true);
        downCheck1.SetActive(true);
        blockingCheck1.SetActive(true);
        cliffhangerCheck1.SetActive(true);

        upCheck2.SetActive(false);
        downCheck2.SetActive(false);
        blockingCheck2.SetActive(false);
        cliffhangerCheck2.SetActive(false);
    }

    void SortDrops()
    {
        dropList.Sort((a, b) => a.rarity.CompareTo(b.rarity));
    }

    [SerializeField] GameObject damageText;
    [SerializeField] Transform damageTextPoint;

    public void DamageNpc(float damage, string damageCause)
    {
        float damageCalc = damage - npc.armor;

        if (damageCalc <= 0)
        {
            damageCalc = 1;
        }

        health -= damageCalc;

        if (damageCause == "physical")
        {
            Vector3 textPos = new Vector3(damageTextPoint.position.x, damageTextPoint.position.y);

            GameObject damageTexts = Instantiate(damageText, textPos, Quaternion.identity);
            damageTexts.GetComponent<TextMeshPro>().text = "" + (int)damageCalc;

            Destroy(damageTexts, 5f);
            anim.SetTrigger("hurt");
        }

        else if (damageCause == "magical")
        {
            Vector3 textPos = new Vector3(damageTextPoint.position.x, damageTextPoint.position.y);

            GameObject damageTexts = Instantiate(damageText, textPos, Quaternion.identity);
            damageTexts.GetComponent<TextMeshPro>().color = new Color(255, 0, 255);
            damageTexts.GetComponent<TextMeshPro>().text = "" + (int)damageCalc;

            Destroy(damageTexts, 5f);
            anim.SetTrigger("hurt");
        }

        else if (damageCause == "poison")
        {
            Vector3 textPos = new Vector3(damageTextPoint.position.x, damageTextPoint.position.y);

            GameObject damageTexts = Instantiate(damageText, textPos, Quaternion.identity);
            damageTexts.GetComponent<TextMeshPro>().color = new Color(0, 255, 0);
            damageTexts.GetComponent<TextMeshPro>().text = "" + (int)damageCalc;

            Destroy(damageTexts, 5f);
            anim.SetTrigger("hurt");
        }
    }

    public void SetAttacker(GameObject attacker)
    {
        this.attacker = attacker;
    }

    [SerializeField] float everySecondTimer;

    private void Update()
    {
        player = GameObject.Find("Player Object");
        spawnerList = GameObject.Find("Spawner List");
        attackTimer += Time.deltaTime;
        chat = player.GetComponent<Chat>();
        dungeonHandler = GameObject.Find("Dungeon Manager").GetComponent<DungeonHandler>();
        worldSettings = GameObject.Find("World Settings").GetComponent<WorldSettings>();
        everySecondTimer += Time.deltaTime;

        for (int i = 0; i < activeDebuff.Count; i++)
        {
            stun = false;
            silence = false;
            blind = false;
            disarm = false;
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

        if (everySecondTimer >= 1)
        {
            everySecondTimer = 0;
            EverySecond();
        }

        if (rightAmount == 0 && leftAmount == 0)
        {
            fixMovementTimer += Time.deltaTime;

            if (fixMovementTimer >= 5)
            {
                canDown = true;
                collisionBlocking = true;
            }
        }

        jumpTimer += Time.deltaTime;

        if (jumpTimer >= 3 && canUp && collisionBlocking)
        {
            jumpTimer = 0;
            rb.velocity = Vector2.up * npc.jumpSpeed;
        }

        SortDrops();
        DebuffModifier();

        if (health <= 0)
        {
            Dead();
        }

        // Health Regen
        if (health <= npc.health)
        {
            health += npc.healthRegen * Time.deltaTime;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        else if (health < maxHealth)
        {
            health += npc.healthRegen;
        }

        AI_AgroBehaviour();
        Attack();
        Move();

        // Enemy NPC Type core Modifier
        if (npc.category == Npc.Category.Enemy)
        {
            distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);

            Collider2D[] closestAllies = Physics2D.OverlapCircleAll(transform.position, npc.followRange, alliesLayer);

            foreach (Collider2D al in closestAllies)
            {
                allies = al.gameObject;
                distanceFromAllies = Vector2.Distance(transform.position, al.transform.position);
                if (distanceFromAllies <= npc.closestTargetRange)
                {
                    allies = al.gameObject;
                }
            }
        }


        // Allies NPC Type core Modifier
        if (npc.category == Npc.Category.Allies)
        {
            Collider2D[] closestEnemy = Physics2D.OverlapCircleAll(transform.position, npc.followRange, enemyLayer);

            foreach (Collider2D en in closestEnemy)
            {
                distanceFromEnemy = Vector2.Distance(transform.position, en.transform.position);

                if (distanceFromEnemy <= npc.closestTargetRange)
                {
                    enemy = en.gameObject;
                }
            }
        }
        
        
    }


    public void DebuffNpc(string name, float amount, float duration)
    {
        DebuffType bType = DebuffType.None;

        if (name == "Stun")
        {
            bType = DebuffType.Stun;
        }

        else if (name == "Slow")
        {
            bType = DebuffType.Slow;
        }

        else if (name == "Silence")
        {
            bType = DebuffType.Silence;
        }

        else if (name == "Blind")
        {
            bType = DebuffType.Blind;
        }

        else if (name == "Root")
        {
            bType = DebuffType.Root;
        }

        else if (name == "Disarm")
        {
            bType = DebuffType.Disarm;
        }

        else if (name == "Poison")
        {
            bType = DebuffType.Poison;
        }

        activeDebuff.Add(bType);
        activeDebuffDuration.Add(duration);
        activeDebuffDamage.Add(duration);
    }


    public void AI_AgroBehaviour()
    {
        if (npc.category == Npc.Category.Enemy)
        {
            if (!agroAllies && distanceFromPlayer <= npc.followRange)
            {
                isFollow = true;
                agroPlayer = true;

                if (distanceFromPlayer <= npc.attackRange)
                {
                    isTarget = true;
                }

                else if (distanceFromPlayer > npc.attackRange)
                {
                    isTarget = false;
                }
            }
            else if (!agroAllies && agroPlayer && distanceFromPlayer > npc.followRange)
            {
                isFollow = false;
                isTarget = false;
                agroPlayer = false;
            }

            else if (!agroPlayer && distanceFromAllies <= npc.followRange)
            {
                isFollow = true;
                agroAllies = true;

                if (distanceFromAllies <= npc.attackRange)
                {
                    isTarget = true;
                }

                else if (distanceFromAllies > npc.attackRange)
                {
                    isTarget = false;
                }
            }
            else if (!agroPlayer && agroAllies && distanceFromAllies > npc.followRange)
            {
                isFollow = false;
                isTarget = false;
                agroAllies = false;
            }
        }

        else if (npc.category == Npc.Category.Allies)
        {
            if (distanceFromEnemy < npc.followRange)
            {
                isFollow = true;

                if (distanceFromEnemy <= npc.attackRange)
                {
                    isTarget = true;
                }

                else if (distanceFromEnemy > npc.attackRange)
                {
                    isTarget = false;
                }
            }
            else
            {
                isFollow = false;
                isTarget = false;
            }
        }

        if (npc.category == Npc.Category.Enemy && allies == null && agroAllies)
        {
            isFollow = false;
            agroAllies = false;
            isTarget = false;
        }
    }

    private void Dead()
    {
        anim.SetTrigger("dead");
        GameObject soundObject = Instantiate(player.GetComponent<SoundManager>().soundBox, transform.position, Quaternion.identity);
        AudioClip deadSound = player.GetComponent<SoundManager>().GetAudioClip("player_gain_experience");
        soundObject.GetComponent<AudioSource>().PlayOneShot(deadSound);
        if (attacker != null && attacker == player)
        {
            //anim.SetTrigger("dead");

            float randomDrops = UnityEngine.Random.Range(1, 101);
            float randomDropsAmount = UnityEngine.Random.Range(npc.level, npc.level * 2);
            float randomExps = UnityEngine.Random.Range(npc.level, npc.level * 5);

            player.GetComponent<PlayerAttribute>().expTextTimer = 3;
            player.GetComponent<PlayerAttribute>().expText.text = "+" + randomExps + " Exp";

            GameObject deathBody = Instantiate(npc.deathBodyObject, transform.position, Quaternion.identity);

            foreach (Item item in dropList)
            {
                if (randomDrops <= item.rarityDrop)
                {

                    if (npc.instantDrop)
                    {

                        GameObject dropItem = player.GetComponent<Inventory>().dropItem;
                        dropItem.GetComponent<DropItem>().item = item;
                        dropItem.GetComponent<SpriteRenderer>().sprite = item.sprite;
                        dropItem.transform.localScale = new Vector2(10, 10);


                        if (item.stackable)
                        {
                            for (int i = 0; i < randomDropsAmount; i++)
                            {
                                Instantiate(dropItem, transform.position, Quaternion.identity);
                            }
                            break;
                        }

                        else if (!item.stackable)
                        {
                            Instantiate(dropItem, transform.position, Quaternion.identity);
                            break;
                        }

                    }

                    if (npc.lootSystem)
                    {

                        deathBody.GetComponent<DeathBody>().npc = npc;
                        deathBody.GetComponent<DeathBody>().lList.Add(item);

                        if (item.stackable)
                        {
                            deathBody.GetComponent<DeathBody>().lCount[item.id] = (int)randomDropsAmount;
                            break;
                        }

                        else if (!item.stackable)
                        {
                            deathBody.GetComponent<DeathBody>().lCount[item.id] = 1;
                            break;
                        }

                        Debug.Log("[NPC-H] Rarity: " + randomDrops);
                        break;

                    }
                }
            }

            if (npc.lootSystem)
            {
                deathBody.GetComponent<DeathBody>().DestroyCorpse(60);
            }

            else if (!npc.lootSystem)
            {
                deathBody.GetComponent<DeathBody>().DestroyCorpse(0);
            }

            player.GetComponent<PlayerAttribute>().AddExp(randomExps);

            enabled = false;

            mobDisplayHandler.canvasObject.SetActive(false);

            foreach (GameObject spawner in spawnerList.GetComponent<SpawnerHandler>().spawnerList)
            {
                if (spawner.GetComponent<Spawner>().id == MOB_SPAWNER_ID)
                {
                    spawner.GetComponent<Spawner>().currentMob--;
                    break;
                }
            }

            if (player.GetComponent<QuestHandler>().quest != null)
            {
                player.GetComponent<QuestHandler>().TriggerQuest(1, npc.name, npc.name);
            }
            TriggerDungeon();
            Destroy(parent, 3f);
        }

        else if (!attacker)
        {
            foreach (GameObject spawner in spawnerList.GetComponent<SpawnerHandler>().spawnerList)
            {
                if (spawner.GetComponent<Spawner>().id == MOB_SPAWNER_ID)
                {
                    spawner.GetComponent<Spawner>().currentMob--;
                    break;
                }
            }
            TriggerDungeon();
            Destroy(parent, 3f);
        }
    }

    [SerializeField] DungeonHandler dungeonHandler;

    public void TriggerDungeon()
    {
        if (worldSettings.dungeon)
        {
            dungeonHandler.currentMob--;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, npc.attackRange);
    }

    [SerializeField] float distanceFromPlayer;
    [SerializeField] float distanceFromEnemy;
    [SerializeField] float distanceFromAllies;
    [SerializeField] GameObject parent;

    private void Move()
    {
        if (!canDown && !cliffHangerBlocking)
        {
            rb.velocity = new Vector2(0, 0);
        }

        if (isFollow)
        {
            if (enemy != null && npc.category == Npc.Category.Allies)
            {
                if (enemy.transform.position.x < transform.position.x && canDown || enemy.transform.position.x < transform.position.x && cliffHangerBlocking)
                {
                    if (isTarget || stun || root)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (!isTarget && !stun && !root)
                    {
                        rb.velocity = new Vector2(-1 * npc.movementSpeed, rb.velocity.y);
                    }
                    LeftCollisionDetector();
                    transform.localScale = new Vector2(bodyScale.x * -1, bodyScale.y);
                }

                else if (enemy.transform.position.x > transform.position.x && canDown || enemy.transform.position.x > transform.position.x && cliffHangerBlocking)
                {
                    if (isTarget || stun || root)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (!isTarget && !stun && !root)
                    {
                        rb.velocity = new Vector2(1 * npc.movementSpeed, rb.velocity.y);
                    }
                    RightCollisionDetector();
                    transform.localScale = new Vector2(bodyScale.x * 1, bodyScale.y);
                }

                anim.Play(npc.name + "_walk");
            }

            else if (npc.category == Npc.Category.Enemy && agroPlayer)
            {
                if (player.transform.position.x < transform.position.x && canDown || player.transform.position.x < transform.position.x && cliffHangerBlocking)
                {
                    if (isTarget || stun || root)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (!isTarget && !stun && !root)
                    {
                        rb.velocity = new Vector2(-1 * npc.movementSpeed, rb.velocity.y);
                    }
                    LeftCollisionDetector();
                    transform.localScale = new Vector2(bodyScale.x * -1, bodyScale.y);
                }

                else if (player.transform.position.x > transform.position.x && canDown || player.transform.position.x > transform.position.x && cliffHangerBlocking)
                {
                    if (isTarget || stun || root)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (!isTarget && !stun && !root)
                    {
                        rb.velocity = new Vector2(1 * npc.movementSpeed, rb.velocity.y);
                    }
                    RightCollisionDetector();
                    transform.localScale = new Vector2(bodyScale.x * 1, bodyScale.y);
                }

                anim.Play(npc.name + "_walk");
            }

            else if (allies != null && npc.category == Npc.Category.Enemy && !agroPlayer)
            {
                if (allies.transform.position.x < transform.position.x && canDown || allies.transform.position.x < transform.position.x && cliffHangerBlocking)
                {
                    if (isTarget || stun || root)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (!isTarget && !stun && !root)
                    {
                        rb.velocity = new Vector2(-1 * npc.movementSpeed, rb.velocity.y);
                    }
                    LeftCollisionDetector();
                    transform.localScale = new Vector2(bodyScale.x * -1, bodyScale.y);
                }

                else if (allies.transform.position.x > transform.position.x && canDown || allies.transform.position.x > transform.position.x && cliffHangerBlocking)
                {
                    if (isTarget || stun || root)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (!isTarget && !stun && !root)
                    {
                        rb.velocity = new Vector2(1 * npc.movementSpeed, rb.velocity.y);
                    }
                    RightCollisionDetector();
                    transform.localScale = new Vector2(bodyScale.x * 1, bodyScale.y);
                }

                anim.Play(npc.name + "_walk");
            }
        }

        else if (!isFollow)
        {
            AI_Behaviour();
        }
    }

    [SerializeField] float changeBehaviourTimer;
    [SerializeField] float changePathfindTimer;
    [SerializeField] float rightRandom;
    [SerializeField] float leftRandom;

    public void AI_Behaviour()
    {
        changeBehaviourTimer += Time.deltaTime;
        changePathfindTimer += Time.deltaTime;

        if (changeBehaviourTimer >= npc.changeBehaviourCooldown)
        {
            changeBehaviourTimer = 0;

            if (right)
            {
                right = false;
                left = true;
            }

            else if (left)
            {
                left = false;
                right = true;
            }
        }

        if (changePathfindTimer >= npc.changePathfindCooldown)
        {
            changePathfindTimer = 0;

            rightRandom = UnityEngine.Random.Range(npc.maxRight, npc.maxRight * 2);
            leftRandom = UnityEngine.Random.Range(npc.maxLeft, npc.maxLeft * 2);

            float rightOrLeftRandom = UnityEngine.Random.Range(1, 10);

            int rightOrLeft = (int) rightOrLeftRandom;

            if (rightOrLeft > 5)
            {
                right = true;
                rightAmount = 0;

                left = false;

                //Debug.Log("[AI] We Got Right");
            }

            else if (rightOrLeft <= 5)
            {
                left = true;
                leftAmount = 0;

                right = false;

                //Debug.Log("[AI] We Got Left");
            }
        }


        if (right && rightAmount < rightRandom && canDown && !stun || right && rightAmount < rightRandom && cliffHangerBlocking && !stun)
        {
            RightCollisionDetector();
            rightAmount += 1 * Time.deltaTime;
            rb.velocity = new Vector2(1 * npc.movementSpeed, rb.velocity.y);
            transform.localScale = new Vector2(bodyScale.x * 1, bodyScale.y);
        }

        else if (left &&  leftAmount < leftRandom && canDown && !stun || left && leftAmount < leftRandom && cliffHangerBlocking && !stun)
        {
            LeftCollisionDetector();
            leftAmount += 1 * Time.deltaTime;
            rb.velocity = new Vector2(-1 * npc.movementSpeed, rb.velocity.y);
            transform.localScale = new Vector2(bodyScale.x * -1, bodyScale.y);
        }
    }

    

    private void Attack()
    {
        //float distance = Vector2.Distance(transform.position, player.transform.position);



        if (attackTimer >= npc.attackSpeed && isTarget && npc.category == Npc.Category.Enemy && agroPlayer)
        {
            attackTimer = 0;

            Collider2D[] castAttack = Physics2D.OverlapCircleAll(attackPoint.position,
                npc.attackRange, playerLayer);

            if (npc.type == Npc.Type.Melee)
            {
                foreach (Collider2D playerLayers in castAttack)
                {
                    float damageRandom = UnityEngine.Random.Range(npc.damageMin, npc.damageMax);

                    float damage = damageRandom;

                    playerLayers.GetComponent<PlayerAttribute>().DamagePlayer(damage);

                    Debug.Log("[NPC-H] Enemy - " + npc.name + " Attacked You: " + damage);
                    anim.SetTrigger("melee");
                    break;
                }
            }



            else if (npc.type == Npc.Type.Range)
            {
                Vector3 playerPosition = player.transform.position;
                playerPosition.z = 0f;

                Vector3 attackPointPosition = transform.position;
                Vector3 direction = (playerPosition - attackPointPosition).normalized;

                GameObject projectile = Instantiate(npc.projectile, attackPointPosition, Quaternion.identity);
                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

                projectile.GetComponent<ProjectileHandler>().target = direction;
                projectile.GetComponent<ProjectileHandler>().ProjectileTimer(npc.attackRange);
                projectile.GetComponent<ProjectileHandler>().speed = npc.projectileSpeed;

                float damageRandom = UnityEngine.Random.Range(npc.damageMin, npc.damageMax);

                float damage = damageRandom;
                projectile.GetComponent<ProjectileHandler>().Damage(damage);
            }
        }


        else if (attackTimer >= npc.attackSpeed && isTarget && npc.category == Npc.Category.Enemy && !agroPlayer)
        {
            attackTimer = 0;

            Collider2D[] castAttack2 = Physics2D.OverlapCircleAll(attackPoint.position,
                npc.attackRange, alliesLayer);

            if (npc.type == Npc.Type.Melee)
            {
                foreach (Collider2D alliesLayers in castAttack2)
                {
                    float damageRandom2 = UnityEngine.Random.Range(npc.damageMin, npc.damageMax);

                    float damage2 = damageRandom2;

                    alliesLayers.GetComponent<NpcHandler>().DamageNpc(damage2, "physical");

                    Debug.Log("[NPC-H] Enemy - " + npc.name + " Attacked You: " + damage2);
                    anim.SetTrigger("melee");
                    break;
                }
            }



            else if (npc.type == Npc.Type.Range)
            {
                Vector3 playerPosition = player.transform.position;
                playerPosition.z = 0f;

                Vector3 attackPointPosition = transform.position;
                Vector3 direction = (playerPosition - attackPointPosition).normalized;

                GameObject projectile = Instantiate(npc.projectile, attackPointPosition, Quaternion.identity);
                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

                projectile.GetComponent<ProjectileHandler>().target = direction;
                projectile.GetComponent<ProjectileHandler>().ProjectileTimer(npc.attackRange);
                projectile.GetComponent<ProjectileHandler>().speed = npc.projectileSpeed;

                float damageRandom = UnityEngine.Random.Range(npc.damageMin, npc.damageMax);

                float damage = damageRandom;
                projectile.GetComponent<ProjectileHandler>().Damage(damage);
            }
        }





        else if (attackTimer >= npc.attackSpeed && isTarget && npc.category == Npc.Category.Allies)
        {
            attackTimer = 0;

            Collider2D[] castAttack = Physics2D.OverlapCircleAll(attackPoint.position,
                npc.attackRange, enemyLayer);

            if (npc.type == Npc.Type.Melee)
            {
                foreach (Collider2D p in castAttack)
                {
                    float damageRandom = UnityEngine.Random.Range(npc.damageMin, npc.damageMax);

                    float damage = damageRandom;

                    p.GetComponent<NpcHandler>().DamageNpc(damage, "physical");
                    p.GetComponent<NpcHandler>().allies = gameObject;

                    Debug.Log("[NPC-H] Enemy - " + npc.name + " Attacked You: " + damage);
                    anim.SetTrigger("melee");
                    break;
                }
            }

            else if (npc.type == Npc.Type.Range)
            {
                Vector3 playerPosition = player.transform.position;
                playerPosition.z = 0f;

                Vector3 attackPointPosition = transform.position;
                Vector3 direction = (playerPosition - attackPointPosition).normalized;

                GameObject projectile = Instantiate(npc.projectile, attackPointPosition, Quaternion.identity);
                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

                projectile.GetComponent<ProjectileHandler>().target = direction;
                projectile.GetComponent<ProjectileHandler>().ProjectileTimer(npc.attackRange);
                projectile.GetComponent<ProjectileHandler>().speed = npc.projectileSpeed;

                float damageRandom = UnityEngine.Random.Range(npc.damageMin, npc.damageMax);

                float damage = damageRandom;
                projectile.GetComponent<ProjectileHandler>().Damage(damage);
            }
        }
    }


    public void BuffModifier()
    {
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

    public void EverySecond()
    {
        // Called Every Second 

        for (int i = 0; i < activeDebuff.Count; i++)
        {
            if (activeDebuff[i] == DebuffType.Poison)
            {
                DamageNpc(activeDebuffDamage[i], "poison");
            }
        }
    }
}
