using UnityEditor;
using UnityEngine;
public class ItemGenerator : EditorWindow
{
    [MenuItem("MyTools/Generate Items")]
    private static void GenerateItems()
    {
        // Define the number of items you want to generate
        int numberOfItems = 1;

        for (int i = 0; i < numberOfItems; i++)
        {
            // Create a new instance of your ScriptableObject
            Item newItem = ScriptableObject.CreateInstance<Item>();

            // Set the properties of the item (customize this based on your needs)
            newItem.name = "" + i;
            newItem.displayname = "Item " + i;

            // Save the ScriptableObject asset
            AssetDatabase.CreateAsset(newItem, "Assets/ScriptableObject/Item/" + newItem.name + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Debug.Log(numberOfItems + " items generated.");
    }
}
