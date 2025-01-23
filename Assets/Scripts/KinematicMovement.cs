using UnityEngine;

public class KinematicMovement : Movement
{
    public override void ApplyForce(Vector3 force) {
        Acceleration += force;
    }

    private void LateUpdate() {
        Velocity += Acceleration * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, movementData.maxSpeed);
        
        transform.position += Velocity * Time.deltaTime;
        Acceleration = Vector3.zero;
    }
}
