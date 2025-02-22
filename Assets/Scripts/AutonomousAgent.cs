using System;
using UnityEngine;

public class AutonomousAgent : AIAgent {
    public Perception perception;
    public ObstaclePerception obstaclePerception;
    public enum MovementType {
        Chase,
        Flee,
        Flock
    }
    public MovementType movementType;
    protected Vector3 recordedPos = Vector3.zero;

    private void Update() {
        Vector3 moveVector = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(GetVelocity().normalized);
        recordedPos = transform.position;

        int its = 0;
        //TARGET PERCEPTION
        foreach (var go in perception.GetTargets()) {
            if (movementType == MovementType.Chase) {
                Vector3 targetVector = go.transform.position - transform.position;
                if (moveVector == Vector3.zero || targetVector.magnitude < moveVector.magnitude) {
                    moveVector = targetVector.normalized;
                }
                its = 1;
            }
            else if (movementType == MovementType.Flock) {
                Vector3 targetVector = transform.position - go.transform.position;
                if (targetVector.magnitude < movement.movementData.maxSpeed * 1.5f) {
                    movementType = MovementType.Flee;
                }
            }
            else if (movementType == MovementType.Flee) {
                Vector3 targetVector = transform.position - go.transform.position;
                if (targetVector.magnitude >= movement.movementData.maxSpeed * 1.5f) {
                    movementType = MovementType.Flock;
                }
                else {
                    moveVector += movement.movementData.maxSpeed * (targetVector.normalized / MathF.Max(targetVector.magnitude * 0.05f,float.MinValue));
                    its++;
                }
            }
            //Debug.DrawLine(transform.position, go.transform.position, Color.magenta);
        }
        
        //NEIGHBOR PERCEPTION
        if (movementType == MovementType.Flock) {
            movement.ApplyForce(Cohesion(perception.GetNeighbors().ToArray()));
            movement.ApplyForce(Separation(perception.GetNeighbors().ToArray(),movement.movementData.maxSpeed * 0.5f));
            movement.ApplyForce(Alignment(perception.GetNeighbors().ToArray()));
        }
        
        its = (its == 0) ? 1 : its;
        moveVector /= its;

        //OBSTACLE PERCEPTION
        //Debug.DrawLine(transform.position, transform.position - transform.forward, Color.magenta);
        if (obstaclePerception != null && obstaclePerception.CheckDirection(-Vector3.forward))
        {
            Vector3 direction = Vector3.zero;
            if (obstaclePerception.GetOpenDirection(ref direction))
            {
                Debug.DrawRay(transform.position, direction, Color.white);
                movement.ApplyForce(direction * movement.movementData.maxSpeed);
            }
        }
        else
        {
            movement.ApplyForce(moveVector * movement.movementData.maxSpeed);
        }
        
        //Debug.DrawLine(transform.position, transform.position + moveVector, Color.magenta);
        transform.position = Utilities.Wrap(transform.position, new Vector3(-30, -30, -30), new Vector3(30, 30, 30));
    }

    private Vector3 Cohesion(GameObject[] neighbors) {
        Vector3 positions = Vector3.zero;
        foreach (var neighbor in neighbors) {
            positions += neighbor.transform.position;
        }

        Vector3 center = positions / neighbors.Length;
        Vector3 direction = center - transform.position;
        Vector3 force = direction.normalized * movement.movementData.minSpeed;

        return force;
    }
    
    private Vector3 Separation(GameObject[] neighbors, float radius) {
        Vector3 separation = Vector3.zero;
        float strength = 10;
        foreach (var neighbor in neighbors)
        {
            Vector3 direction = transform.position - neighbor.transform.position;
            float distance = direction.magnitude;
            if (distance < radius)
            {
                separation += direction;
                strength = (strength > distance && distance > 0) ? distance : strength;
            }
        }

        Vector3 force = (separation.normalized * (movement.movementData.maxSpeed * 5f)) / strength;
        //Debug.DrawLine(transform.position, transform.position + force, Color.magenta);

        return force;
    }
    
    private Vector3 Alignment(GameObject[] neighbors) {
        Vector3 velocities = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            Vector3 velocity = neighbor.GetComponent<AutonomousAgent>().GetVelocity();
            velocities += velocity.normalized;
        }
        velocities += GetVelocity();
        
        Vector3 force = (velocities / (neighbors.Length+1)) * (movement.movementData.maxSpeed * 1f);
        force.y = 0;
        //Debug.DrawLine(transform.position, transform.position + force, Color.magenta);

        return force;
    }

    private Vector3 GetVelocity()
    {
        return recordedPos - transform.position;
    }
}
