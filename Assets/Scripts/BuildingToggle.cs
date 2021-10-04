using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingToggle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("BUILDING: " + other.gameObject.tag);

        if (other.gameObject.tag == "Table")
        {
            Debug.LogError("CHANGE!");
            GameManager.instance.buildingLayer1.SetActive(!GameManager.instance.buildingLayer1.activeSelf);
            GameManager.instance.buildingLayer2.SetActive(!GameManager.instance.buildingLayer2.activeSelf);
        }
    }
}
