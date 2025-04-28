using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractNpcHandler : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Npc npc;
    [SerializeField] Chat chat;
    [SerializeField] Quest quest;

    [SerializeField] GameObject questUI;
    [SerializeField] GameObject npcMenuUI;
    [SerializeField] GameObject NAMETAG;
    [SerializeField] GameObject bodyObject;
    [SerializeField] GameObject notificationGameObject;

    public float distanceFromMouse;
    public float distanceFromPlayer;

    float minimumRange;

    private void Awake()
    {
        minimumRange = 15;
    }

    private void Update()
    {
        player = GameObject.Find("Player Object");
        quest = npc.quest;
        npcMenuUI = player.GetComponent<QuestHandler>().npcMenuUI;
        chat = player.GetComponent<Chat>();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        distanceFromMouse = Vector2.Distance(mousePosition, transform.position);
        distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);

        NAMETAG.transform.position = bodyObject.transform.position; 

        if (distanceFromPlayer <= minimumRange)
        {
            notificationGameObject.SetActive(true);
        }

        else if (distanceFromPlayer >= minimumRange)
        {
            notificationGameObject.SetActive(false);
        }

        Interact();
    }

    private void Interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (distanceFromMouse <= minimumRange && distanceFromPlayer <= minimumRange)
            {
                npcMenuUI.SetActive(true);
                player.GetComponent<QuestHandler>().quest = quest;
                player.GetComponent<QuestHandler>().clickedNpc = npc.displayname;
            }
        }
    }
}
