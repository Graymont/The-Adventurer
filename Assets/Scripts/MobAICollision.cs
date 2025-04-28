using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAICollision : MonoBehaviour
{
    [SerializeField] NpcHandler npcHandler;

    [SerializeField] bool up;

    [SerializeField] bool down;

    [SerializeField] bool blocking;
    [SerializeField] bool cliffhanger;

    private void Awake()
    {

        npcHandler.canUp = true;
        npcHandler.canDown = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (up && !blocking && !cliffhanger)
        {
            if (collision.gameObject.tag == "Ground")
            {
                npcHandler.canUp = false;
            }
        }

        else if (down && !blocking && !cliffhanger)
        {
            if (collision.gameObject.tag == "Ground")
            {
                npcHandler.canDown = true;
            }
        }



        else if (blocking && !cliffhanger && !up && !down)
        {
            if (collision.gameObject.tag == "Ground")
            {
                npcHandler.collisionBlocking = true;
            }
        }

        else if (cliffhanger && !blocking && !up && !down)
        {
            if (collision.gameObject.tag == "Ground")
            {
                npcHandler.cliffHangerBlocking = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (up && !blocking && !cliffhanger)
        {
            if (collision.gameObject.tag == "Ground")
            {
                npcHandler.canUp = true;
            }
        }

        else if (down && !blocking && !cliffhanger)
        {
            if (collision.gameObject.tag == "Ground")
            {
                npcHandler.canDown = false;
            }
        }

        else if (blocking && !cliffhanger && !up && !down)
        {
            if (collision.gameObject.tag == "Ground")
            {
                npcHandler.collisionBlocking = false;
            }
        }

        else if (cliffhanger && !blocking && !up && !down)
        {
            if (collision.gameObject.tag == "Ground")
            {
                npcHandler.cliffHangerBlocking = false;
            }
        }

    }
}
