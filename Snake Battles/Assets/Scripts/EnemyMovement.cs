using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public SnakeMovement SnakeMovement;
    public SnakeTail SnakeTail;
    public SnakeGrow SnakeGrow;

    public EnemyTail EnemyTail;

    // All set to private as not used in other scripts
    private float enemySpeed = 3f;

    private float distance;
    private float distanceBetween = 10;

    // Built-in function: Called on first frame
    private void Start()
    {

    }

    // Built-in Function: Called every frame
    private void Update()
    {
        DistanceFromNearestSnake();
    }

    private void DistanceFromNearestSnake()
    {
        // Checks if either the player or enemy has been killed before running the code to prevent possible errors
        if (SnakeTail.snakeAlive && EnemyTail.enemyAlive)
        {
            // Calculate the direction vector from the enemy to the target (SnakeTail)
            Vector3 direction = SnakeTail.SnakeTailObj.transform.position - transform.position;

            // Desired angle (adjust this as needed)
            float desiredAngle = 45f;

            // Rotate the direction vector by the desired angle
            direction = RotateVector(direction, desiredAngle).normalized;

            // Movement - Moves the enemy towards the player (SnakeTail) at the specified speed
            transform.position = Vector2.MoveTowards(transform.position, SnakeTail.SnakeTailObj.transform.position, enemySpeed * Time.deltaTime);

            // Rotation - Rotate the enemy to face the target direction at a certain speed
            RotateTowardsTarget(direction);
        }
    }

    // Rotates the vector by a specific angle
    private Vector3 RotateVector(Vector3 vec, float angle)
    {
        float radians = Mathf.Deg2Rad * angle;
        float x = vec.x * Mathf.Cos(radians) - vec.y * Mathf.Sin(radians);
        float y = vec.x * Mathf.Sin(radians) + vec.y * Mathf.Cos(radians);
        return new Vector3(x, y, vec.z); // Keep the z-component the same
    }

    // Rotate the enemy towards the target direction
    private void RotateTowardsTarget(Vector3 direction)
    {
        // Calculate the rotation angle (in degrees) to turn towards the target
        float step = SnakeMovement.rotateSpeed * Time.deltaTime;

        // Create a rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

        // Smoothly rotate towards the target direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
    }
}
