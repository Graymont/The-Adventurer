using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestHandler : MonoBehaviour
{
    public Quest quest;
    public Chat chat;
    public PlayerAttribute playerAttribute;
    public Storage storage;
    public ItemStatsDisplayer isd;
    public Inventory inventory;
    public int max = 10;
    public int selected;
    public string clickedNpc;
    public string currentTarget;
    public List<Quest> qSlot = new List<Quest>();
    public List<int> qCount = new List<int>();

    [Header ("Quest UI")]
    public List<GameObject> questContentList = new List<GameObject>();
    public List<TextMeshProUGUI> questTitleText = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> questCompleteText = new List<TextMeshProUGUI>();

    [Header("Quest Npc UI")]
    public GameObject npcMenuUI;
    public GameObject questInfoGameObject;
    public TextMeshProUGUI questInfoTitle;
    public TextMeshProUGUI questInfoStory;
    public TextMeshProUGUI questInfoDescription;
    public TextMeshProUGUI questInfoNpcName;
    public TextMeshProUGUI questInfoExp;
    public List<GameObject> questInfoRewadList = new List<GameObject>();
    public List<Image> questInfoRewardImage = new List<Image>();

    public GameObject acceptButton;
    public GameObject completeButton;
    

    [SerializeField] GameObject questListGameObject;
    bool questListActive;

    public void PressTalk()
    {

    }

    public void PressQuest()
    {
        OpenQuestInfo(quest);
    }

    public void CloseQuestList()
    {
        questListActive = false;
    }

    public void OnButtonReward(int index)
    {
        isd.itemDisplayObjectEnable = true;
        isd.CalculateStatsDisplay(quest.rewards[index]);
    }

    public void OpenQuestInfo(Quest theQuest)
    {
        questInfoGameObject.SetActive(true);
        quest = theQuest;

        // Content Set

        questInfoTitle.text = quest.displayname;
        questInfoStory.text = quest.story;
        questInfoDescription.text = quest.description;
        questInfoNpcName.text = clickedNpc;
        questInfoExp.text = "[EXP: "+quest.expReward+" | Shell: "+quest.shellReward+"]";

        // Reward Set

        for (int i = 0; i < 6; i++)
        {
            if (i >= quest.rewards.Count)
            {
                questInfoRewadList[i].SetActive(false);
            }

            else if (i < quest.rewards.Count)
            {
                questInfoRewadList[i].SetActive(true);
                questInfoRewardImage[i].sprite = quest.rewards[i].sprite;
            }
        }

        foreach (Quest q in qSlot)
        {
            acceptButton.SetActive(true);
            if (q == quest)
            {
                acceptButton.SetActive(false);
                break;
            }
        }

        foreach (int count in qCount)
        {
            completeButton.SetActive(false);
            if (count >= quest.amount)
            {
                completeButton.SetActive(true);
                break;
            }
        }
    }

    private void Awake()
    {
        //questListActive = true;

        questInfoGameObject.SetActive(false);
    }

    private void Update()
    {
        questListGameObject.SetActive(questListActive);

        foreach (Quest qs in qSlot)
        {
            if (qs.type == Quest.Type.Delivery)
            {
                if (inventory.iCount[qs.deliveryItem.id] > qCount[qs.id])
                {
                    TriggerQuest(1, qs.name, qs.targetName);
                }
            }
        }
    }

    public void OpenQuestInfo()
    {
        if (questListActive)
        {
            questListActive = false;
        }

        else if (!questListActive)
        {
            questListActive = true;
        }
    }

    public void AcceptQuest()
    {
        if (playerAttribute.level >= quest.level)
        {
            string name = quest.name;

            AddQuest(name);
        }

        else if (playerAttribute.level < quest.level)
        {
            chat.AddText("Your level is too low!", Color.red);
        }
        questInfoGameObject.SetActive(false);
        OpenQuestInfo(quest);
    }

    public void SelectQuest(int number)
    {
        selected = number;
    }

    /*
     0 = Kill Quest
     1 = Delivery Quest
     2 = Interaction Quest 
    */

    public void TriggerQuest(int category, string name, string target)
    {
        foreach (Quest quest in qSlot)
        {
            if (category == 0 && quest.name == name || quest.targetName == target)
            {
                qCount[quest.id]++;
                Debug.Log("Quest <" + quest.name + ">" + " [" + qCount[quest.id] + "/" + quest.amount + "]");
            }

            else if (category == 1 && quest.name == name || quest.targetName == name)
            {
                if (qCount[quest.id] < inventory.iCount[quest.deliveryItem.id])
                {
                    qCount[quest.id] = inventory.iCount[quest.deliveryItem.id];
                }

                else if (qCount[quest.id] > inventory.iCount[quest.deliveryItem.id])
                {
                    qCount[quest.id] = inventory.iCount[quest.deliveryItem.id];
                }
            }

            else if (category == 2 && quest.name == name || quest.targetName == name)
            {
                qCount[quest.id] = 1;
            }
        }
    }

    public void CompleteQuest()
    {
        if (quest != null)
        {
            if (quest.type == Quest.Type.Kill)
            {
                if (qCount[quest.id] >= quest.amount)
                {
                    qCount[quest.id] = 0;
                    GiveReward();
                }
            }

            else if (quest.type == Quest.Type.Delivery)
            {
                if (qCount[quest.deliveryItem.id] >= quest.amount)
                {
                    qCount[quest.id] = 0;
                    inventory.iSlot.Remove(quest.deliveryItem);
                    inventory.iCount[quest.deliveryItem.id] -= quest.amount;
                    GiveReward();
                }
            }

            else if (quest.type == Quest.Type.Interaction)
            {
                if (currentTarget == quest.targetName)
                {
                    qCount[quest.id] = 0;
                    GiveReward();
                }
            }
        }
    }

    public void GiveReward()
    {
        foreach (Quest q in storage.questList)
        {
            if (q.name == quest.name)
            {
                for (int i = 0; i < q.rewards.Count; i++)
                {
                    for (int a = 0; a < q.rewardAmount[i]; a++)
                    {
                       inventory.AddItem(q.rewards[i].name);
                    }
                }
                playerAttribute.exp += quest.expReward;
                playerAttribute.shell += quest.shellReward;

                quest = null;             
                qSlot.Remove(q);

                chat.AddText("---------------", Color.gray);
                chat.AddText("[+"+quest.expReward+" Exp]", Color.yellow);
                chat.AddText("[+" + quest.shellReward + " Shell]", Color.white);
                chat.AddText("---------------", Color.gray);

                break;
            }
        }
    }

    public void AddQuest(string name)
    {
        foreach (Quest quest in storage.questList)
        {
            if (quest.name == name && !qSlot.Contains(quest))
            {
                qSlot.Add(quest);
                chat.AddText("Quest Added:"+quest.displayname, Color.blue);
                chat.AddText("[Quest Book Updated]", Color.gray);
                break;
            }
            else
            {
                chat.AddText("You've already started this quest!", Color.red);
                break;
            }
        }
    }
}
