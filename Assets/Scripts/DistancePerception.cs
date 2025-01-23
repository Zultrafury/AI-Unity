using System;
using System.Collections.Generic;
using UnityEngine;

public class DistancePerception : Perception
{
    public override List<GameObject> GetTargets() {
        List<GameObject> tempgo = new List<GameObject>();
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        string searchTag = CompareTag("Agent01") ? "Agent02" : CompareTag("Agent02") ? "Agent01" : "NO TAG";
        foreach (GameObject go in allObjects) {
            if (go.CompareTag(searchTag)) {
                tempgo.Add(go);
            }
        }

        return tempgo;
    }

    public override List<GameObject> GetNeighbors() {
        List<GameObject> tempgo = new List<GameObject>();
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        string searchTag = gameObject.tag;
        foreach (GameObject go in allObjects) {
            if (go.CompareTag(searchTag)) {
                tempgo.Add(go);
            }
        }

        return tempgo;
    }
}
