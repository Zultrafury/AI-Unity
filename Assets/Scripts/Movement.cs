using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Movement : MonoBehaviour {
    public MovementData movementData;

    public Vector3 Velocity { get; set; }
    public Vector3 Acceleration { get; set; }
    public Vector3 Direction {
        get { return Velocity.normalized; }
    }
    
    public abstract void ApplyForce(Vector3 force);
}
