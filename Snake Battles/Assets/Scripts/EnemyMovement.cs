using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public SnakeMovement SnakeMovement;
    public SnakeTail SnakeTail;
    public SnakeGrow SnakeGrow;

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

    // Everything is stored as a method so they can be easily changed to public & be accessible to other scripts if needed, improving their flexibility, reusability & readability of the code

    private void DistanceFromNearestSnake()
    {
        // Calculates the distance between the player's snake (for now) & the enemy snake
        distance = Vector2.Distance(transform.position, SnakeTail.SnakeTailObj.transform.position);

        // If the player (for now) gets closer than 10 on the x & y axis to the enemy snake, the enemy snake will chase after them
        if (distance < distanceBetween)
        {
            // Movement - Moves the snake towards the player snake (for now) at the same speed as the player's snake
            transform.position = Vector2.MoveTowards(transform.position, SnakeTail.SnakeTailObj.transform.position, enemySpeed * Time.deltaTime);

            // Rotation - Rotates the snake at the same speed as the player snake
            transform.Rotate(SnakeMovement.rotateSpeed * Time.deltaTime * -SnakeMovement.velocityX * Vector3.forward);

        }
    }
}
