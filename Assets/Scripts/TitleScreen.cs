using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public int saveSlot;


    private void Update()
    {
        
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("MainScene");
    }
}
