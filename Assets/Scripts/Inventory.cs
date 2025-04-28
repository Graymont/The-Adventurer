
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Inventory : MonoBehaviour
{
    public Storage storage;
    [SerializeField] WorldSettings worldSettings;
    [SerializeField] SoundManager soundManager;

    [SerializeField] PlayerController playerController;

    [SerializeField] ItemStatsDisplayer itemStatsDisplayer;

    [SerializeField] GameObject inventory;
    public GameObject dropItem;

    [SerializeField] Transform dropPoint;

    [SerializeField] Sprite inventoryEmptySprite;

    [SerializeField] Chat chat;
    
    
    public bool inventoryActive;
    
    public int max = 36;
    public int selected;
    public int eSelected;

    public List<Item> iSlot = new List<Item>();
    public List<int> iCount = new List<int>();

    public List<Button> iButton = new List<Button>();
    public List<Button> eButton = new List<Button>();
    public List<Image> eImage = new List<Image>();
    public List<Sprite> eSprite = new List<Sprite>();
    public List<Image> iImage = new List<Image>();
    public List<TextMeshProUGUI> iText = new List<TextMeshProUGUI>();

    private void Awake()
    {
        worldSettings = GameObject.Find("World Settings").GetComponent<WorldSettings>();
        soundManager = GetComponent<SoundManager>();
    }

    public void Drop()
    {
        dropItem.GetComponent<DropItem>().item = iSlot[selected];   
        dropItem.GetComponent<SpriteRenderer>().sprite = iSlot[selected].sprite;
        chat.AddText("Dropped Item: " + iSlot[selected].displayname, Color.green);

        Vector2 dropItemSize = new Vector2(10, 10);

        dropItem.transform.localScale = dropItemSize;

        Instantiate(dropItem, dropPoint.position, Quaternion.identity);

        if (iSlot[selected].stackable)
        {
            iCount[iSlot[selected].id]--;
        }

        else if (!iSlot[selected].stackable)
        {
            iCount[iSlot[selected].id]--;
            iSlot.RemoveAt(selected);
        }
    }

    public void Equip()
    {
        if (iSlot[selected].type == Item.Type.Hand)
        {
            playerController.hand.GetComponent<ItemHandler>().item = iSlot[selected];
        }

        if (iSlot[selected].type == Item.Type.Head)
        {
            playerController.head.GetComponent<ItemHandler>().item = iSlot[selected];
        }

        if (iSlot[selected].type == Item.Type.Body)
        {
            playerController.body.GetComponent<ItemHandler>().item = iSlot[selected];
        }

        if (iSlot[selected].type == Item.Type.Feet)
        {
            playerController.feet.GetComponent<ItemHandler>().item = iSlot[selected];
        }
        soundManager.PlaySound("player_equip", 1, 1);
    }

    public void Unequip()
    {
        Debug.Log("[INV] Unequip");
        if (eSelected == 0)
        {
            playerController.hand.GetComponent<ItemHandler>().item = null;
        }

        if (eSelected == 1)
        {
            playerController.head.GetComponent<ItemHandler>().item = null;
        }

        if (eSelected == 2)
        {
            playerController.body.GetComponent<ItemHandler>().item = null;
        }

        if (eSelected == 3)
        {
            playerController.feet.GetComponent<ItemHandler>().item = null;
        }
        soundManager.PlaySound("player_unequip", 1, 1);
    }

    public void OnEquipmentClick(int number)
    {
        eSelected = number;
        soundManager.PlaySound("player_inventory_slot_click", 1, 1);
    }

    public void OnButtonClick(int number)
    {
        if (number == selected)
        {
            if (number < iSlot.Count && iSlot.Count > 0)
            {
                itemStatsDisplayer.itemDisplayObjectEnable = true;

                if (selected < iSlot.Count && iSlot.Count > 0)
                {
                    itemStatsDisplayer.CalculateStatsDisplay(iSlot[selected]);
                }

                for (int i = 0; i < 8; i++)
                {
                    itemStatsDisplayer.bonusStats[i].gameObject.SetActive(false);
                }
            }
        }

        else if (number != selected)
        {
            itemStatsDisplayer.itemDisplayObjectEnable = false;
        }
        selected = number;
        soundManager.PlaySound("player_inventory_slot_click", 1, 1);
    }

    private void Update()
    {
        //Sort();

        for (int i = 0; i < iSlot.Count; i++)
        {
            if (iCount[iSlot[i].id] <= 0 && iSlot[i].stackable){
                iSlot.RemoveAt(i);
            }
        }


        if (playerController.hand.GetComponent<ItemHandler>().item != null)
        {
            eImage[0].sprite = playerController.hand.GetComponent<ItemHandler>().item.inventoryDisplay;
        }
        else
        {
            eImage[0].sprite = eSprite[0];
        }

        if (playerController.head.GetComponent<ItemHandler>().item != null)
        {
            eImage[1].sprite = playerController.head.GetComponent<ItemHandler>().item.inventoryDisplay;
        }
        else
        {
            eImage[1].sprite = eSprite[1];
        }

        if (playerController.body.GetComponent<ItemHandler>().item != null)
        {
            eImage[2].sprite = playerController.body.GetComponent<ItemHandler>().item.inventoryDisplay;
        }
        else
        {
            eImage[2].sprite = eSprite[2];
        }

        if (playerController.feet.GetComponent<ItemHandler>().item != null)
        {
            eImage[3].sprite = playerController.feet.GetComponent<ItemHandler>().item.inventoryDisplay;
        }
        else
        {
            eImage[3].sprite = eSprite[3];
        }


        for (int i = 0; i < iSlot.Count; i++)
        {
            if (iText[i].gameObject.activeSelf)
            {
                if (iSlot[i].stackable)
                {
                    iText[i].text = "" + iCount[iSlot[i].id];
                }

                else if (!iSlot[i].stackable)
                {
                    iText[i].text = "" + 1;
                }
            }
            iImage[i].sprite = iSlot[i].inventoryDisplay;
        }


        for (int i = 0; i < 36; i++)
        {
            if (selected != i)
            {
                Color colors = iImage[i].color;

                colors.a = 1;

                iImage[i].color = colors;
            }

            if (i >= iSlot.Count)
            {
                iText[i].gameObject.SetActive(false);
                iImage[i].sprite = inventoryEmptySprite;
            }

            else if (i < iSlot.Count)
            {
                iText[i].gameObject.SetActive(true);
            }

            if (iSlot.Count <= 0)
            {
                iText[0].gameObject.SetActive(false);
                iImage[0].sprite = inventoryEmptySprite;
            }
        }

        Color color = iImage[selected].color;

        color.a = 125f / 255f;

        iImage[selected].color = color;

        for (int i = 0; i < 4; i++)
        {
            if (eSelected != i)
            {
                Color colors2 = eImage[i].color;

                colors2.a = 1;

                eImage[i].color = colors2;
            }
        }

        Color color2 = eImage[eSelected].color;

        color2.a = 125f / 255f;

        eImage[eSelected].color = color2;

        if (Input.GetKeyDown(KeyCode.I)){
            OpenInventory();
        }
        inventory.SetActive(inventoryActive);
    }

    public void OpenInventory()
    {
        if (inventoryActive)
        {
            Sort();
            inventoryActive = false;
            playerController.uiInteracting = false;
        }

        else
        {
            Sort();
            inventoryActive = true;
            playerController.uiInteracting = true;
        }
    }

    public void Sort()
    {
        iSlot.Sort((a, b) => a.sort.CompareTo(b.sort));
    }

    public void AddItem(string name)
    {
        Item itemGet = null;

        bool hasItem = false;

        if (iSlot.Count < max)
        {
            foreach (Item item in storage.itemList)
            {
                if (item.name == name)
                {
                    itemGet = item;
                    break;
                }
            }

            if (itemGet != null)
            {
                foreach (Item item in iSlot)
                {
                    if (itemGet.name == item.name)
                    {
                        hasItem = true;
                        break;
                    }
                }

                if (itemGet.stackable)
                {
                    if (hasItem)
                    {
                        iCount[itemGet.id]++;
                    }

                    else if (!hasItem)
                    {
                        iSlot.Add(itemGet);
                        iCount[itemGet.id]++;
                    }
                }

                else if (!itemGet.stackable)
                {
                    iSlot.Add(itemGet);
                    iCount[itemGet.id]++;
                }
            }
            else
            {
                Debug.LogWarning("[INV] Item with name " + name + " not found!");
            }
        }
        else
        {
            Debug.LogError("[INV] Inventory is Full!");
        }

    }
}
