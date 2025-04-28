using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Inventory inventory;
    public PlayerAttribute playerAttribute;
    public SoundManager soundManager;
    public LayerMask enemyLayer;
    
    Vector2 bodyScale;

    public Animator anim;
    public GameObject groundDetector;

    public float movementSpeed;
    public float jumpSpeed;

    public bool canJump;
    public bool canDoubleJump;

    public bool isSprint;
    public bool uiInteracting;

    [Header("Item Manager")]

    public float attackTimer;
    public GameObject head;
    public GameObject hand;
    public GameObject body;
    public GameObject feet;


    [Header("Camera Adjust")]

    public bool cameraZoomIn;
    public bool cameraZoomOut;

    public float cameraZoomSpeed = 10;
    public float cameraZoomInMax = 25;
    public float cameraZoomOutMax = 75;

    public CinemachineVirtualCamera cam;

    [Header("Mob Info UI")]
    public GameObject mobInfoUI;
    public Image mobInfoPotrait;
    public TextMeshProUGUI mobInfoName;
    public TextMeshProUGUI mobInfoLevel;
    public TextMeshProUGUI mobInfoHealth;

    [Header ("Mining")]
    [SerializeField] Vector3 mousePosition;
    [SerializeField] Transform miningPoint;
    [SerializeField] GameObject miningGameObject;
    [SerializeField] LayerMask miningLayer;
    [SerializeField] float miningRange;

    private void Awake()
    {
        bodyScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cam = GameObject.Find("Camera").GetComponent<CinemachineVirtualCamera>();

        worldSettings = GameObject.Find("World Settings").GetComponent<WorldSettings>();
    }

    private void Update()
    {
        float hi = Input.GetAxisRaw("Horizontal");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        movementSpeed = 35+playerAttribute.movementSpeed;

        CameraAdjust();

        if (isSprint && !uiInteracting && !playerAttribute.root && !playerAttribute.stun) 
        {
            rb.velocity = new Vector2(hi * movementSpeed * 1.35f, rb.velocity.y);
        }
        else if (!isSprint && !uiInteracting && !playerAttribute.root && !playerAttribute.stun)
        { 
            rb.velocity = new Vector2(hi * movementSpeed, rb.velocity.y);
        }

        if (uiInteracting || playerAttribute.root || playerAttribute.stun)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (hi > 0 && !uiInteracting)
        {
            transform.localScale = new Vector2(bodyScale.x, bodyScale.y);
        }

        else if (hi < 0 && !uiInteracting)
        {
            transform.localScale = new Vector2(-bodyScale.x, bodyScale.y);
        }

        anim.SetBool("walk", hi != 0);
        //miningGameObject.transform.localPosition = mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            Melee();
            Range();

            //Mining();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !uiInteracting && !playerAttribute.root && !playerAttribute.stun)
        {
            if (groundDetector.GetComponent<GroundDetector>().touchGround)
            {              
                rb.velocity = Vector2.up * jumpSpeed;
                anim.SetTrigger("Jump");
                soundManager.PlaySound("player_jump", 1, 1);
            }

            else if (groundDetector.GetComponent<GroundDetector>().touchGround2 && !playerAttribute.root && !playerAttribute.stun)
            {
                groundDetector.GetComponent<GroundDetector>().touchGround2 = false;
                rb.velocity = Vector2.up * jumpSpeed;
                anim.SetTrigger("Jump");
                soundManager.PlaySound("player_jump", 1, 1);
            }
        }

        MouseMobShowInfo();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprint = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprint = false;
        }


        attackTimer += Time.deltaTime;

        Cheat();
    }

    [SerializeField] WorldSettings worldSettings;

    void Mining()
    {
        Collider2D[] castMining = Physics2D.OverlapCircleAll(miningPoint.position, miningRange, miningLayer);

        foreach (Collider2D block in castMining)
        {
            Destroy(block.gameObject);
        }
    }

    void Cheat()
    {
        if (worldSettings.cheat)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                inventory.AddItem("ironsword");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                inventory.AddItem("smallhealingpotion");
            }
        }
    }

    public void Melee()
    {
        if (hand.GetComponent<ItemHandler>().item != null 
            && hand.GetComponent<ItemHandler>().item.combatStyle == Item.CombatStyle.Melee && !uiInteracting && !playerAttribute.disarm && !playerAttribute.stun)
        {
            if (attackTimer >= hand.GetComponent<ItemHandler>().item.attackSpeed / 100 + playerAttribute.attackSpeed/100)
            {
                attackTimer = 0;
                anim.SetTrigger("Melee");
                soundManager.PlaySound("player_melee_attack", 1, 3);

                Collider2D[] drawAttack = Physics2D.OverlapCircleAll(hand.GetComponent<ItemHandler>().attackPoint_melee.position,
                    hand.GetComponent<ItemHandler>().item.attackRange, hand.GetComponent<ItemHandler>().enemyLayer);

                foreach (Collider2D enemy in drawAttack)
                {
                    // Attack

                    float damageRandom = UnityEngine.Random.Range(hand.GetComponent<ItemHandler>().item.damageMin, hand.GetComponent<ItemHandler>().item.damageMax);

                    float damage = damageRandom+playerAttribute.strength;                  

                    enemy.GetComponent<NpcHandler>().DamageNpc(damage, "physical");
                    enemy.GetComponent<NpcHandler>().SetAttacker(gameObject);
                    Debug.Log("[PC] Melee Attacl: "+ damage);
                }     
            }
        }

    }

    public void Range()
    {
        if (hand.GetComponent<ItemHandler>().item != null 
            && hand.GetComponent<ItemHandler>().item.combatStyle == Item.CombatStyle.Range && !uiInteracting && !playerAttribute.disarm && !playerAttribute.stun)
        {
            if (attackTimer >= hand.GetComponent<ItemHandler>().item.attackSpeed/100 + playerAttribute.attackSpeed/100)
            {
                attackTimer = 0;
                anim.SetTrigger("Range");

                if (hand.GetComponent<ItemHandler>().item.category == Item.Category.BOW)
                {
                    soundManager.PlaySound("player_bow_shoot", 1, 1);
                }

                else if (hand.GetComponent<ItemHandler>().item.category == Item.Category.STAFF)
                {
                    soundManager.PlaySound("player_staff_shoot", 1, 1);
                }

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f; 

                Vector3 attackPointPosition = hand.GetComponent<ItemHandler>().attackPoint_range.position;
                Vector3 direction = (mousePosition - attackPointPosition).normalized;

                GameObject projectile = Instantiate(hand.GetComponent<ItemHandler>().item.projectile, attackPointPosition, Quaternion.identity);
                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

                projectile.GetComponent<ProjectileHandler>().target = direction;  
                projectile.GetComponent<ProjectileHandler>().ProjectileTimer(hand.GetComponent<ItemHandler>().item.attackRange);
                projectile.GetComponent<ProjectileHandler>().speed = hand.GetComponent<ItemHandler>().item.projectileSpeed;

                float damageRandom = UnityEngine.Random.Range(hand.GetComponent<ItemHandler>().item.damageMin, hand.GetComponent<ItemHandler>().item.damageMax);
                float damage = damageRandom+playerAttribute.dexterity;

                projectile.GetComponent<ProjectileHandler>().Damage(damage);
                projectile.GetComponent<ProjectileHandler>().SetAttacker(gameObject);
            }
        }
    }


    public void Dash()
    {
        Rigidbody2D rbd = GetComponent<Rigidbody2D>();

        rbd.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
    }

    public void MouseMobShowInfo()
    {
        GameObject targetNpc = null;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] checkMouse = Physics2D.OverlapCircleAll(mousePos, 10f, enemyLayer);

        foreach (Collider2D tn in checkMouse)
        {
            float distanceFromMob = Vector2.Distance(mousePos, tn.transform.position);

            if (distanceFromMob <= 5)
            {
                targetNpc = tn.gameObject;
            }
        }
        if (targetNpc != null)
        {
            mobInfoUI.SetActive(true);
            mobInfoPotrait.sprite = targetNpc.GetComponent<NpcHandler>().npc.potrait;
            mobInfoName.text = targetNpc.GetComponent<NpcHandler>().npc.displayname;
            mobInfoLevel.text = "Level " + targetNpc.GetComponent<NpcHandler>().npc.level;
            mobInfoHealth.text = (int)targetNpc.GetComponent<NpcHandler>().health / targetNpc.GetComponent<NpcHandler>().maxHealth * 100 + "%";

            if (targetNpc.GetComponent<NpcHandler>().health > targetNpc.GetComponent<NpcHandler>().maxHealth / 2)
            {
                mobInfoHealth.color = new Color(Color.green.r, Color.green.g, Color.green.b);
            }

            else if (targetNpc.GetComponent<NpcHandler>().health > targetNpc.GetComponent<NpcHandler>().maxHealth / 4)
            {
                mobInfoHealth.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b);
            }

            else if (targetNpc.GetComponent<NpcHandler>().health < targetNpc.GetComponent<NpcHandler>().maxHealth / 4)
            {
                mobInfoHealth.color = new Color(Color.red.r, Color.red.g, Color.red.b);
            }
        }

        else if (targetNpc == null)
        {
            mobInfoUI.SetActive(false);
        }
    }

    public void CameraAdjust()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            cameraZoomIn = true;
        }

        if (Input.GetKeyUp(KeyCode.Equals))
        {
            cameraZoomIn = false;
        }

        if (cameraZoomIn && cam.m_Lens.OrthographicSize > cameraZoomInMax)
        {
            cam.m_Lens.OrthographicSize -= cameraZoomSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            cameraZoomOut = true;
        }

        if (Input.GetKeyUp(KeyCode.Minus))
        {
            cameraZoomOut = false;
        }

        if (cameraZoomOut && cam.m_Lens.OrthographicSize < cameraZoomOutMax)
        {
            cam.m_Lens.OrthographicSize += cameraZoomSpeed * Time.deltaTime;
        }
    }

}
