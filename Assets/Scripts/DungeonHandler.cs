using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHandler : MonoBehaviour
{
    public Dungeon dungeon;

    public GameObject player;
    public GameObject bossSpawnPoint;
    public GameObject bossArea;
    public GameObject bossObject;
    public int currentMob;

    private void Awake()
    {
        bossArea = GameObject.Find("Boss Area");
        bossSpawnPoint = GameObject.Find("Boss Spawn Point");
        currentMob = dungeon.totalMob;
    }

    private void Start()
    {
        if (dungeon.dungeonType == Dungeon.DungeonType.Raid)
        {
            Instantiate(dungeon.boss, bossSpawnPoint.transform.position, Quaternion.identity);
        }
    }
    bool gotoBoss;
    bool completed;
    private void Update()
    {
        player = GameObject.Find("Player Object");
        if (dungeon.dungeonType == Dungeon.DungeonType.Raid)
        {
            if (currentMob <= 0 && !gotoBoss)
            {
                CompleteDungeon();
            }

            // health <= 0 -> mati

            if (bossObject.GetComponent<NpcHandler>().health <= 0 && !completed)
            {
                FinishDungeon();
            }
        }
    }

    public void CompleteDungeon()
    {
        gotoBoss = true;
        Vector3 bossAreaSP = bossArea.transform.position;
        player.transform.position = new Vector3(bossAreaSP.x, bossAreaSP.y, bossAreaSP.z);
    }

    public void FinishDungeon()
    {
        completed = true;
        for (int i = 0; i < dungeon.rewards.Count; i++)
        {
            for (int a = 0; a < dungeon.rewardAmount[i]; a++)
            {
                player.GetComponent<Inventory>().AddItem(dungeon.rewards[i].name);
            }
        }
        Debug.Log("Dungeon Finished!");
    }
}
