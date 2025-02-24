using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public RandomSpawn RandomSpawn;
    public EnemyTail EnemyTail;
    public KillSnake KillSnake;

    public GameObject Snake;

    public Transform SnakeTailObj;
    public Transform SnakeHeadObj;

    public List<Transform> snakeTail = new List<Transform>();
    public List<Vector2> positions = new List<Vector2>();

    public bool snakeAlive = true;

    // All set to private as not used in other scripts
    private float circDiameter = 0.5f;

    private float timeElapsed = 0f;
    private float timeEnd = 0.5f;
    private bool isTimerActive = false;

    // Built-in function: Called on first frame
    private void Start()
    {
        InitialisePositions();
    }

    // Built-in Function: Called every frame
    private void Update()
    {
        // Referenced in Update() with an null check as the Enemy(Clone) GameObject is spawned in runtime and doesn't exist before the program is ran
        if (EnemyTail == null && snakeAlive)
        {
            EnemyTail = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyTail>();
        }

        CalculatePositions();
        RunTimer();
    }

    // Everything is stored as a method so they can be easily changed to public & be accessible to other scripts if needed, improving their flexibility, reusability & readability of the code

    private void InitialisePositions()
    {
        // Initialise the first position (the head's position)
        positions.Add(SnakeTailObj.position);

        // Initialises the starting size of the snake to 5
        for (int i = 0; i < 4; i++)
        {
            AddTail();
        }

        // Sets the first tail object to be the head of the tail, used as part of KillSnake
        SnakeHeadObj = snakeTail[0];
    }

    private void CalculatePositions()
    {
        // Checks if the player has been killed before running the code to prevent possible errors
        if (snakeAlive)
        {
            // Calculate the distance between the head and the last segment to be inserted, .magnitude used to make the value read-only
            float distance = ((Vector2)SnakeTailObj.position - positions[0]).magnitude;

            // Keeps all tail positions of the snake in sync with the head's position
            if (distance > circDiameter)
            {
                // Calculates the direction for all tails to follow, .normalised used to prevent unwanted faster movement when the snake moves diagonally
                Vector2 direction = ((Vector2)SnakeTailObj.position - positions[0]).normalized;

                // Moves the first position forward by a fixed amount according to circDiameter
                positions.Insert(0, positions[0] + direction * circDiameter);
                positions.RemoveAt(positions.Count - 1);

                // Resets the distance after modifying positions, -= used to prevent potential visual errors
                distance -= circDiameter;
            }

            // Updates each tails segment position, using .Lerp to smoothen this movement
            for (int i = 0; i < snakeTail.Count; i++)
            {
                snakeTail[i].position = Vector2.Lerp(positions[i + 1], positions[i], distance / circDiameter);
            }
        }
    }

    public void AddTail()
    {
        // Checks if the player has been killed before running the code to prevent possible errors
        if (snakeAlive)
        {
            // Instantiates a new tail segment and position at the last position in the list positions
            Transform tail = Instantiate(SnakeTailObj, positions[positions.Count - 1], Quaternion.identity, transform);
            // Added to both lists to avoid inconsistencies between the two that may lead to inaccurate calculations
            snakeTail.Add(tail);
            positions.Add(tail.position);
        }
    }

    private void DestroyTail()
    {
        // If there are 5 or more instances of the snake's tail, run this code
        if (snakeTail.Count >= 5)
        {
            // References the end tail attatched to the snake
            Transform lastTail = snakeTail[snakeTail.Count - 1];

            // Removes the end tail's instance from snakeTail & position from positions
            snakeTail.RemoveAt(snakeTail.Count - 1);
            positions.RemoveAt(positions.Count - 1);

            // Actually destroys the object
            Destroy(lastTail.gameObject);

            // Adjusts the size GUI to take 1 away from the current size displayed
            RandomSpawn.RemoveFoodFromSizeCounter();
        }
    }

    // Instead of destroying the player object, clone objects of SnakeTail are instead removed as the game is intended to be replayable in runtime
    public void KillPlayer()
    {
        // Sets snakeAlive to false to prevent the function CalculatePositions running when it can't calculate the player's position
        snakeAlive = false;

        // Gets all SnakeTail(Clone) objects which are all stored in snakeTail
        for (int i = 0; i < snakeTail.Count; i++)
        {
            // If the name of the object at the current index of snakeTail is true, run this code
            // This will always be true as all clones of SnakeTail will be given this exact name
            if (snakeTail[i].gameObject.name == "SnakeTail(Clone)")
            {
                // Destroys the gameObject connected to the transform of the current index inside snakeTail
                Destroy(snakeTail[i].gameObject);
                // Removes each index in turn from the list
                snakeTail.RemoveAt(i);
                // Adjusts the index after removal to avoid skipping the next index
                i--;
            }
        }

        // Clears all indexes in the list positions
        positions.Clear();
    }

    // The entire purpose of this function is to reset the game so it can replayed in runtime
    public void GameOver()
    {
        // Pauses the game so nothing is moving once the game ends
        Time.timeScale = 0;

        // Creates a local list called objectsWithFoodTag that will automatically update as food objects are deleted
        GameObject[] objectsWithFoodTag = GameObject.FindGameObjectsWithTag("Food");

        // Since this will contain every food object, it is looped through to essentially clear all food objects from all lists
        foreach (GameObject obj in objectsWithFoodTag)
        {
            // Destroys the GameObjects
            Destroy(obj);
        }

        // Clears all lists
        RandomSpawn.foodList.Clear();
        KillSnake.enemyOrbs.Clear();
        KillSnake.enemyOrbPositions.Clear();
        // Kills enemy & resets size counter
        if (EnemyTail != null)
        {
            EnemyTail.KillEnemy();
        }
        RandomSpawn.InitialiseSizeCounter();
    }

    private void RunTimer()
    {
        // If the key LeftShift is pressed, run this code
        if (Input.GetKey(KeyCode.LeftShift))
        {
            StartTimer();
        }

        // If the key LeftShift is lifted/ not held, run this code
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopTimer();
        }
    }

    private void StartTimer() 
    {
        // Set to true so if loop condition is true
        isTimerActive = true;

        if (isTimerActive)
        {
            // Adds up time as a float, one frame at a time as the condition is called in the update function
            timeElapsed += Time.deltaTime;

            // If timeElapsed becomes greater or equal to timeEnd (set to 0.5), destroy the last tail of the snake object & reset the timeElapsed counter to 0
            if (timeElapsed >= timeEnd)
            {
                DestroyTail();
                timeElapsed = 0f;              
            }
        }
    }

    private void StopTimer()
    {
        // Set to false to stop the if statement inside the function StartTimer() from running & resets the counter time Elapsed
        isTimerActive = false;
        timeElapsed = 0f;
    }
}