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

    private float distanceFromTarget;
    private float maxTargetDistance = 10;
    private float minTargetDistance = 2;

    private float distanceFromTail;
    private float nearestTail = 1000;
    private float minTailDistance = 3;

    private Vector2 targetPosition;
    private Vector2 moveBesideDirection;

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
            // Calculates the distance between the player's head (for now) & the enemy's head as the target
            distanceFromTarget = Vector2.Distance(EnemyTail.EnemyHeadObj.transform.position, SnakeTail.SnakeHeadObj.transform.position);

            // Loops through each of the player's tail positions to find the nearest tail
            for (int i = (SnakeTail.positions).Count - 1; i >= 0; i--)
            {
                // Gets the distance between the enemy's head and the current tail position
                distanceFromTail = Vector2.Distance(EnemyTail.EnemyHeadObj.transform.position, SnakeTail.positions[i]);

                // If the value of distanceFromTail is less than the current value of nearestTail, the value of nearestTail is updated
                if (distanceFromTail < nearestTail)
                {
                    nearestTail = distanceFromTail;
                    // Stores the position of the index that is in nearestTail to use as the target
                    if (nearestTail == distanceFromTail)
                    {
                        targetPosition = SnakeTail.positions[i];
                    }
                }
            }

            // If nearestTail is > minTargetDistance, run the enemy movement code below
            if (nearestTail > minTargetDistance)
            {
                // If the distance to the nearest tail is less than minDistance, move parallel to the player's tail
                if (nearestTail < minTailDistance)
                {
                    // Calculates the direction to the player's head and moves parallel to the player's tail
                    Vector2 directionToHead = ((Vector2)SnakeTail.SnakeHeadObj.transform.position - (Vector2)transform.position).normalized;

                    // Rotates 90 degrees away from the player's tail to move besides it
                    moveBesideDirection = new Vector2(-directionToHead.y, directionToHead.x);

                    // Moves the enemy at a perpendicular angle to the player's tail
                    transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + moveBesideDirection, enemySpeed * Time.deltaTime);
                }

                else if (nearestTail < maxTargetDistance)
                {
                    // Otherwise, the enemy chases the player’s head
                    transform.position = Vector2.MoveTowards(transform.position, SnakeTail.SnakeHeadObj.transform.position, enemySpeed * Time.deltaTime);
                }
            }

            // If nearestTail is < minDistance, run the enemy movement code below
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, SnakeTail.SnakeHeadObj.transform.position, enemySpeed * Time.deltaTime);
            }

            // Used in both scenarios so is placed after conditions

            // Rotation - Rotates the snake at the same speed as the player snake - Used in both scenarios so is placed after conditions
            transform.Rotate(SnakeMovement.rotateSpeed * Time.deltaTime * -SnakeMovement.velocityX * Vector3.forward);

            // Wipes the value of nearestTail after the check to prevent retaining the value
            nearestTail = 1000;
        }
    }
}


