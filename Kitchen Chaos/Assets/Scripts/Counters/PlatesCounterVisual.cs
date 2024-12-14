using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{

    [SerializeField]
    private PlatesCounter platesCounter;
    [SerializeField]
    private Transform counterTopPoint;
    [SerializeField]
    private Transform platesVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlatesSpawned += PlatesCounter_OnPlatesSpawned;
        platesCounter.OnPlatesRemoved += PlatesCounter_OnPlatesRemoved;
    }

    private void PlatesCounter_OnPlatesRemoved(object sender, EventArgs e)
    {
        if(plateVisualGameObjectList.Count > 0)
        {
            //There is atleast one plate visual
            GameObject plategameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
            plateVisualGameObjectList.Remove(plategameObject);
            //Destroying last visual
            Destroy(plategameObject);
        }
    }

    private void PlatesCounter_OnPlatesSpawned(object sender, EventArgs e)
    {
        Transform platesVisualTransform = Instantiate(platesVisualPrefab, counterTopPoint);

        float plateOffsetY = 0.1f;
        platesVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);
        plateVisualGameObjectList.Add(platesVisualTransform.gameObject);
    }
}
