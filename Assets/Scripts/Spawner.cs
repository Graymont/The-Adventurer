using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject npc;

    public float max;
    public int currentMob;
    public int id;

    public float delayBetweenSpawn;
    public Transform spawnPoint;
    float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= delayBetweenSpawn && currentMob < max)
        {
            spawnTimer = 0;
            currentMob++;
            GameObject spawnedMob = Instantiate(npc, spawnPoint.position, Quaternion.identity);
            spawnedMob.GetComponentInChildren<NpcHandler>().MOB_SPAWNER_ID = id;
            Debug.Log("Mob Spawned at " + spawnPoint.position);
        }
    }

}
