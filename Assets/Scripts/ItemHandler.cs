using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public Item item;
    public Transform attackPoint_melee;
    public Transform attackPoint_range;

    public GameObject rightFeet;
    public GameObject leftFeet;

    public LayerMask enemyLayer;
    public SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (item != null  && item.type != Item.Type.Feet)
        {
            if (!item.idleAnimated)
            {
                sr.sprite = item.sprite;
            }

            if (item.repost)
            {
                transform.localPosition = item.post;
            }

            if (item.resize)
            {
                transform.localScale = item.size;
            }
        }

        if (item != null && item.type == Item.Type.Feet)
        {
            if (!item.idleAnimated)
            {
                rightFeet.GetComponent<SpriteRenderer>().sprite = item.sprite;
                leftFeet.GetComponent<SpriteRenderer>().sprite = item.sprite;
            }

            if (item.repost)
            {
                rightFeet.transform.localPosition = item.post;
                leftFeet.transform.localPosition = item.post2;
            }

            if (item.resize)
            {
                rightFeet.transform.localScale = item.size;
                leftFeet.transform.localScale = item.size2;
            }
        }

        if (item == null)
        {
            sr.sprite = null;

            if (gameObject.name == "GlobalFeet")
            {
                rightFeet.GetComponent<SpriteRenderer>().sprite = null;
                leftFeet.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (item != null)
        {
            if (item.type == Item.Type.Hand)
            {
                Gizmos.DrawWireSphere(attackPoint_melee.position, item.attackRange);
            }
        }
        else
        {
            return;
        }
        
    }
}
