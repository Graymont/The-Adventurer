using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
   [SerializeField] GameObject player;
    public Item item;

    private void Awake()
    {
        player = GameObject.Find("Player Object");
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= 1)
        {
            player.GetComponent<Inventory>().AddItem("" + item.name);
            Destroy(gameObject);
        }
    }
}
