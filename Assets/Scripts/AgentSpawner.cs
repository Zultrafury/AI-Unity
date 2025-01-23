using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField] AIAgent[] agents;
    [SerializeField] LayerMask layermask;
    public int agentindex = 0;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            agentindex = (agentindex + 1) % agents.Length;
        }
        
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 offset = new Vector3(0,0.5f,0);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, layermask)) {
                Instantiate(agents[agentindex], hit.point + offset, Quaternion.identity);
            }
        }
    }
}
