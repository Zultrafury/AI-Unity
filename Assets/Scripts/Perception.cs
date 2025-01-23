using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Perception : MonoBehaviour {
    public abstract List<GameObject> GetTargets();
    public abstract List<GameObject> GetNeighbors();
}
