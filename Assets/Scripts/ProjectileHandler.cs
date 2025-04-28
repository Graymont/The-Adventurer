using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    float damage;

    public float speed;

    public Rigidbody2D rb;

    public Vector3 target;

    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask alliesLayer;
    [SerializeField] float attackRange;

    public bool enemy;
    public bool player;
    public bool allies;

    public GameObject attacker;

    public void Damage(float damage)
    {
        this.damage = damage;
    }

    public void SetAttacker(GameObject attacker)
    {
        this.attacker = attacker;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ProjectileTimer(float range)
    {
        Destroy(gameObject, range);
    }

    private void FixedUpdate()
    {
        rb.velocity = target * speed;

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90f);
    }

    private void Update()
    {
        if (enemy)
        {
            Collider2D[] drawAttack = Physics2D.OverlapCircleAll(transform.position,
            attackRange, enemyLayer);

            foreach (Collider2D enemy in drawAttack)
            {
                enemy.GetComponent<NpcHandler>().DamageNpc(damage, "physical");
                enemy.GetComponent<NpcHandler>().SetAttacker(attacker);
                Destroy(gameObject);
            }
        }

        if (player)
        {
            Collider2D[] drawAttack = Physics2D.OverlapCircleAll(transform.position,
            attackRange, playerLayer);

            foreach (Collider2D player in drawAttack)
            {
                player.GetComponent<PlayerAttribute>().DamagePlayer(damage);
                player.GetComponent<NpcHandler>().SetAttacker(attacker);
                Destroy(gameObject);
            }
        }

        if (allies)
        {
            Collider2D[] drawAttack = Physics2D.OverlapCircleAll(transform.position,
            attackRange, alliesLayer);

            foreach (Collider2D allies in drawAttack)
            {
                allies.GetComponent<NpcHandler>().DamageNpc(damage, "physical");
                allies.GetComponent<NpcHandler>().SetAttacker(attacker);
                Destroy(gameObject);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
