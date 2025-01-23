using System;
using UnityEngine;

public class AutonomousAgent : AIAgent {
    public Perception perception;
    public enum MovementType {
        Chase,
        Flee,
        Flock
    }
    public MovementType movementType;

    private void Update() {
        Vector3 moveVector = Vector3.zero;
        int its = 0;
        //TARGET PERCEPTION
        foreach (var go in perception.GetTargets()) {
            if (movementType == MovementType.Chase) {
                Vector3 targetVector = go.transform.position - transform.position;
                if (moveVector == Vector3.zero || targetVector.magnitude < moveVector.magnitude) {
                    moveVector = targetVector;
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
                    moveVector += transform.position - go.transform.position;
                    its++;
                }
            }
            Debug.DrawLine(transform.position, go.transform.position, Color.magenta);
        }
        
        //NEIGHBOR PERCEPTION
        if (movementType == MovementType.Flock) {
            movement.ApplyForce(Cohesion(perception.GetNeighbors().ToArray()));
        }
        
        its = (its == 0) ? 1 : its;
        moveVector /= its;

        movement.ApplyForce(moveVector.normalized * movement.movementData.minSpeed);
        transform.position = Utilities.Wrap(transform.position, new Vector3(-10, -10, -10), new Vector3(10, 10, 10));
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
    
    private Vector3 Separation(GameObject[] neighbors) {
        return Vector3.zero;
    }
    
    private Vector3 Alignment(GameObject[] neighbors) {
        return Vector3.zero;
    }
}
