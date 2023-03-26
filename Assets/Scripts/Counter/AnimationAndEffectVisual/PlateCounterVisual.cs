using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] PlatesCounter platesCounter;
    [SerializeField] Transform plateGameObject;
    [SerializeField] Transform topPointSpawn;

    List<GameObject> platesVisualGameObjectList;
    private void Awake()
    {
        platesVisualGameObjectList = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
        platesCounter.OnPlateSpawn += PlatesCounter_OnPlateSpawn;
    }

    private void PlatesCounter_OnPlateSpawn()
    {
        Transform plateVisualTransform = Instantiate(plateGameObject,topPointSpawn);

        // pile the plates by increase the height of the plates
        float plateOffSetY = .1f * platesVisualGameObjectList.Count;
        plateVisualTransform.localPosition += new Vector3(0, plateOffSetY, 0);

        platesVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }

    private void PlatesCounter_OnPlateRemoved()
    {
        // save the last plate in the pile of plates
        GameObject lastPlateObject = platesVisualGameObjectList[platesVisualGameObjectList.Count - 1];
        platesVisualGameObjectList.Remove(lastPlateObject);
        Destroy(lastPlateObject);
    }
}
