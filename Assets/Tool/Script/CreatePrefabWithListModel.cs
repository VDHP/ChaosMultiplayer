using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatePrefabWithListModel : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> modelList; // Public field to assign list of models in Inspector

    [SerializeField]
    private string prefabFolderPath = "Assets/Prefabs"; // Default prefab folder path (configurable)

    [MenuItem("GameObject/Create Prefab With List Model", false, 0)] // Custom menu item
    public static void CreatePrefab()
    {
        CreatePrefabWithListModel prefabCreator = FindObjectOfType<CreatePrefabWithListModel>(); // Find existing instance with list

        // If not found, create a temporary GameObject with the script
        if (prefabCreator == null)
        {
            GameObject tempGameObject = new GameObject("CreatePrefabWithListModelTemp");
            prefabCreator = tempGameObject.AddComponent<CreatePrefabWithListModel>();
        }

        prefabCreator.CreatePrefabInternal(); // Call internal creation function
    }

    public void CreatePrefabInternal()
    {
        if (modelList == null || modelList.Count == 0)
        {
            Debug.LogError("No models found in the list. Please assign models in the Inspector.");
            return;
        }

        // Create empty parent GameObject
        GameObject parentGameObject;

        // Iterate through models and create children
        foreach (GameObject model in modelList)
        {
            parentGameObject = new GameObject(model.name);

            if (model == null)
            {
                Debug.LogError("A null reference exists in the model list. Please check your models.");
                continue;
            }

            GameObject childGameObject = Instantiate(model, parentGameObject.transform);
            childGameObject.transform.localPosition = Vector3.zero; // Reset local position for cleaner hierarchy
                                                                    // Set parent transform as root, effectively making it a prefab
            parentGameObject.transform.SetParent(null);

            // Create prefab asset (updated for Unity 2021)
            PrefabUtility.SaveAsPrefabAsset(parentGameObject, prefabFolderPath + "/" + model.name + ".prefab");

            // Cleanup (optional)
            DestroyImmediate(parentGameObject); // Destroy object after prefab creation (optional)
        }
        modelList.Clear();
    }
}