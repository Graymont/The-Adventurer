using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();
    public List<Quest> questList = new List<Quest>();

    public Item GetItemByName(string name)
    {
        Item i = itemList[0];
        foreach (Item item in itemList)
        {
            if(item.name == name)
            {
                i = item;
            }
        }
        return i;
    }

    public Item GetItemByID(int id)
    {
        Item i = itemList[0];
        foreach (Item item in itemList)
        {
            if (item.id == id)
            {
                i = item;
            }
        }
        return i;
    }
}
