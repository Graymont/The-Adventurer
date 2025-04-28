using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] public List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();


    private void Update()
    {

    }

    public void AddText(string text, Color color)
    { 
        for (int i = 0; i < textList.Count - 1; i++)
        {
            textList[i].text = textList[i + 1].text;
            textList[i].color = textList[i + 1].color;
        }

        textList[textList.Count - 1].color = new Color(color.r, color.g, color.b);
        textList[textList.Count - 1].text = text;
    }

}
