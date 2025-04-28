using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownNpc : MonoBehaviour
{
    public GameObject player;

    [SerializeField] Vector2 bodyScale;

    private void Awake()
    {
        bodyScale = transform.localScale;
        player = GameObject.Find("Player Object");
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 5)
        {
            if (transform.position.x < player.transform.position.x)
            {
                transform.localScale = new Vector2(bodyScale.x, bodyScale.y);
            }

            else if (transform.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector2(-bodyScale.x, bodyScale.y);
            }
        }
    }
}
