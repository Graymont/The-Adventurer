using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobDisplayHandler : MonoBehaviour
{
    [SerializeField] NpcHandler npcHandler;

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider healthTakeDamageSlider;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI nameText;
    public GameObject canvasObject;

    private void Update()
    {
        healthSlider.maxValue = npcHandler.maxHealth;
        healthSlider.value = npcHandler.health;
        healthTakeDamageSlider.maxValue = npcHandler.maxHealth;

        if (healthTakeDamageSlider.value > healthSlider.value)
        {
            healthTakeDamageSlider.value -= 5 * Time.deltaTime;
        }

        else if (healthTakeDamageSlider.value < healthSlider.value)
        {
            healthTakeDamageSlider.value = healthSlider.value;
        }

        healthText.text = (int)npcHandler.health + "/" + (int)npcHandler.maxHealth;
        nameText.text = "" + npcHandler.npc.displayname;

        canvasObject.transform.position = transform.position;

    }
}
