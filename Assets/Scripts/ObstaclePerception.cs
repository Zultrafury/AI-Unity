using System.Collections.Generic;
using UnityEngine;

public class ObstaclePerception : MonoBehaviour
{
    [Range(0, 50)] public float obstacleWeight;
    [Range(0, 20)] public float maxDistance;
    [Range(0, 360)] public int numRaycast;
    [Range(0, 360)] public float maxAngle;
    public LayerMask layerMask;
    public string tagName;
    
    public GameObject[] GetGameObjects()
    {
        // create result list
        List<GameObject> result = new();

        // get array of directions using Utilities.GetDirectionsInCircle
        Vector3[] directions = Utilities.GetDirectionsInCircle(numRaycast, maxAngle);
        // iterate through directions
        foreach (var direction in directions)
        {
            // create ray from transform postion in the direction of (transform.rotation * direction)
            Ray ray = new Ray(transform.position, transform.rotation * -direction);
            Debug.DrawLine(ray.origin, ray.GetPoint(maxDistance), Color.magenta);
            // raycast ray
            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, layerMask))
            {
                // check if collision is self, skip if so
                //if (raycastHit.collider.gameObject == gameObject) continue;
                // check tag, skip if tagName != "" and !CompareTag
                //if (tagName != "" && !raycastHit.collider.CompareTag(tagName)) continue;

                // add game object to results
                result.Add(raycastHit.collider.gameObject);
            }
        }

        // convert list to array
        return result.ToArray();
    }
    
    public bool GetOpenDirection(ref Vector3 openDirection)
    {
        // get array of directions using Utilities.GetDirectionsInCircle
        Vector3[] directions = Utilities.GetDirectionsInCircle(numRaycast, maxAngle);
        // iterate through directions
        foreach (var direction in directions)
        {
            // cast ray from transform position in the direction of (transform.rotation * direction)
            Ray ray = new Ray(transform.position, transform.rotation * -direction);
            // if there is NO raycast hit then that is an open direction
            if (!Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, layerMask))
            {
                // set open direction
                openDirection = new Vector3(ray.direction.x,0,ray.direction.z);
                openDirection = openDirection.normalized * obstacleWeight;
                return true;
            }
        }

        // no open direction
        return false;
    }

    public bool CheckDirection(Vector3 direction)
    {
        foreach (var go in GetGameObjects())
        {
            //Ray ray = new Ray(transform.position, go.transform.position - transform.position);
            //Debug.DrawLine(ray.origin, ray.GetPoint(maxDistance), Color.green, 1.0f);
            return true;
        }
        //Debug.DrawLine(ray.origin, ray.GetPoint(maxDistance), Color.red);
        return false;
    }
}