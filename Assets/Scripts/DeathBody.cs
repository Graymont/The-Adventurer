using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DeathBody : MonoBehaviour
{
    public Npc npc;

    public GameObject player;

    public GameObject lootListGameObject;
    public Inventory inventory;

    public List<Button> lButton = new List<Button>();
    public List<Image> lImage = new List<Image>();
    public List<TextMeshProUGUI> lText = new List<TextMeshProUGUI>();
    public List<GameObject> lGameObject = new List<GameObject>();
    public List<Item> lList = new List<Item>();
    public List<int> lCount = new List<int>(); 

    private void Awake()
    {
        player = GameObject.Find("Player Object");
        inventory = player.GetComponent<Inventory>();
    }

    public void OnLootClick(int number)
    {
        for (int i = 0; i < lCount[number]; i++)
        {
            inventory.AddItem(lList[number].name);
        }

        lList.RemoveAt(number);
    }

    public void DestroyCorpse(int time)
    {
        Destroy(gameObject, time);
    }

    private void Update()
    {
        //lootListGameObject.transform.position = transform.position;

        if (npc != null)
        {
            GetComponent<SpriteRenderer>().sprite = npc.deathBodySprite;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= 5)
        {
            lootListGameObject.SetActive(true);
        }
        else if (distance > 5)
        {
            lootListGameObject.SetActive(false);
        }

        for (int i = 0; i < 8; i++)
        {
            if (i >= lList.Count)
            {
                lGameObject[i].gameObject.SetActive(false);
            }

            else if (i < lList.Count)
            {
                lGameObject[i].gameObject.SetActive(true);
                lImage[i].sprite = lList[i].sprite;
                lText[i].text = "[x" + lCount[lList[i].id]+"]" +" "+ lList[i].displayname;
            }
        }
    }
}
