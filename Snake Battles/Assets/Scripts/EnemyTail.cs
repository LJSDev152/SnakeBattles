using System.Collections.Generic;
using UnityEngine;

public class EnemyTail : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public RandomSpawn RandomSpawn;

    public GameObject Enemy;

    public Transform EnemyTailObj;
    public Transform EnemyHeadObj;

    public List<Transform> enemyTail = new List<Transform>();
    public List<Vector2> enemyPositions = new List<Vector2>();

    public bool enemyAlive = true;

    // Set to private as not used in other scripts
    private float circDiameter = 0.5f;

    // Built-in function: Called on first frame
    private void Start()
    {
        // Referenced at the start as when an enemy is cloned, the scripts that were previously dragged on in the inspector no longer exist and must be referenced like this instead
        RandomSpawn = GameObject.FindGameObjectWithTag("Snake").GetComponent<RandomSpawn>();

        InitialiseEnemyPositions();
        // Sets the first tail object to be the head of the tail, used as part of KillSnake
        EnemyHeadObj = enemyTail[0];
    }

    // Built-in Function: Called every frame
    private void Update()
    {
        CalculateEnemyPositions();
    }

    // Everything is stored as a method so they can be easily changed to public & be accessible to other scripts if needed, improving their flexibility, reusability & readability of the code

    private void InitialiseEnemyPositions()
    {
        // Initialise the first position (the head's position)
        enemyPositions.Add(EnemyTailObj.position);

        // Initialises the starting size of the snake to 5
        for (int i = 0; i < 4; i++)
        {
            AddEnemyTail();
        }
    }

    private void CalculateEnemyPositions()
    {
        // Checks if the enemy has been killed before running the code to prevent possible errors
        if (enemyAlive)
        {
            // Calculate the distance between the head and the last segment to be inserted, .magnitude used to make the value read-only
            float distance = ((Vector2)EnemyTailObj.position - enemyPositions[0]).magnitude;

            // Keeps all tail positions of the snake in sync with the head's position
            if (distance > circDiameter)
            {
                // Calculates the direction for all tails to follow, .normalised used to prevent unwanted faster movement when the snake moves diagonally
                Vector2 direction = ((Vector2)EnemyTailObj.position - enemyPositions[0]).normalized;

                // Moves the first position forward by a fixed amount according to circDiameter
                enemyPositions.Insert(0, enemyPositions[0] + direction * circDiameter);
                enemyPositions.RemoveAt(enemyPositions.Count - 1);

                // Resets the distance after modifying positions, -= used to prevent potential visual errors
                distance -= circDiameter;
            }

            // Updates each tails segment position, using .Lerp to smoothen this movement
            for (int i = 0; i < enemyTail.Count; i++)
            {
                enemyTail[i].position = Vector2.Lerp(enemyPositions[i + 1], enemyPositions[i], distance / circDiameter);
            }
        }
    }

    public void AddEnemyTail()
    {
        // Checks if the enemy has been killed before running the code to prevent possible errors
        if (enemyAlive)
        {
            // Instantiates a new tail segment and position at the last position in the list positions
            Transform eTail = Instantiate(EnemyTailObj, enemyPositions[enemyPositions.Count - 1], Quaternion.identity, transform);
            // Added to both lists to avoid inconsistencies between the two that may lead to inaccurate calculations
            enemyTail.Add(eTail);
            enemyPositions.Add(eTail.position);
        }
    }

    public void DestroyEnemyTail()
    {
        // If there are 5 or more instances of the snake's tail, run this code
        if (enemyTail.Count >= 5)
        {
            // References the end tail attatched to the snake
            Transform eLastTail = enemyTail[enemyTail.Count - 1];

            // Removes the end tail's instance from snakeTail & position from positions
            enemyTail.RemoveAt(enemyTail.Count - 1);
            enemyPositions.RemoveAt(enemyPositions.Count - 1);

            // Actually destroys the object
            Destroy(eLastTail.gameObject);
        }
    }

    public void KillEnemy()
    {
        // Sets enemyAlive to false to prevent the function CalculateEnemyPositions running when it can't calculate the enemy's position
        enemyAlive = false;
        // Destroys the enemy snake game object
        Destroy(Enemy);
    }
}

