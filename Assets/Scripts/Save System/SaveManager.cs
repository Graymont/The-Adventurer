using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public Inventory inventory;
    public string dataPath = "/Resources/";
    public string fileName = "PlayerDatabase";
    public string fileExtension = ".json";
    private void Start()
    {
        //LoadData();
    }

    public void SaveData(int fileNumber)
    {
        string filePath = Application.dataPath + dataPath + fileName + fileNumber + fileExtension;
        string[] saveFiles = Directory.GetFiles(Application.dataPath+dataPath, fileName + "*"+fileExtension);
        bool createNewFile = true;

        for (int i = 0; i < saveFiles.Length; i++)
        {
            if (saveFiles[i] == fileName + fileNumber + fileExtension)
            {
                createNewFile = false;
                break;
            }
        }

        if (createNewFile)
        {
            using (File.Create(filePath)) { }
        }

        PlayerData pd = new PlayerData();

        for (int i = 0; i < inventory.iSlot.Count; i++)
        {
            pd.itemID[i] = inventory.iSlot[i].id;
            pd.itemAmount[inventory.iSlot[i].id] = inventory.iCount[inventory.iSlot[i].id];
        }

        string json = JsonUtility.ToJson(pd, true);
        File.WriteAllText(filePath, json);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData(1);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            LoadData(1);
        }
    }

    public void LoadData(int fileNumber)
    {
        if (File.Exists(Application.dataPath+dataPath+fileName+fileNumber+fileExtension))
        {
            string json = File.ReadAllText(Application.dataPath+dataPath+fileName+fileNumber+fileExtension);
            PlayerData pd = JsonUtility.FromJson<PlayerData>(json);

            for (int i = 0; i < pd.itemID.Length; i++)
            {
                for (int a = 0; a < pd.itemAmount[pd.itemID[i]]; a++)
                {
                    Item b = inventory.storage.GetItemByID(pd.itemID[i]);
                    inventory.AddItem(b.name);
                }
            }
        }
    }

    public void ChooseSaveFile(int fileNumber)
    {
        SaveData(fileNumber);
    }

    public void ChooseLoadFile(int fileNumber)
    {
        LoadData(fileNumber);
    }
}
