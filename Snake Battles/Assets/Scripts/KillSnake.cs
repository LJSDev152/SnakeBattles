using System.Collections.Generic;
using UnityEngine;
// Reference for Random
using Random = UnityEngine.Random;

public class KillSnake : MonoBehaviour
{
    public SnakeTail SnakeTail;
    public EnemyTail EnemyTail;
    public RandomSpawn RandomSpawn;

    public List<GameObject> enemyOrbs = new List<GameObject>();

    public List<Vector2> enemyOrbPositions = new List<Vector2>();
    
    private int choice;
    private bool spawningFinished = false;
    private bool notCalled = true;

    // Built-in function: Called on first frame
    private void Start()
    {
        // Referenced at the start as when an enemy is cloned, the scripts that were previously dragged on in the inspector no longer exist and must be referenced like this instead
        SnakeTail = GameObject.FindGameObjectWithTag("Snake").GetComponent<SnakeTail>();
        RandomSpawn = GameObject.FindGameObjectWithTag("Snake").GetComponent<RandomSpawn>();
    }

    // Built-in Function: Called every frame
    private void Update()
    {
        // Referenced in Update() with an null check as the Enemy(Clone) GameObject is spawned in runtime and doesn't exist before the program is ran
        if (EnemyTail == null && notCalled)
        {
            EnemyTail = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyTail>();
            notCalled = false;
            Debug.Log("Called");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If both are ran then both snakes are killed
        // If one is run then the one that is run will only kill that snake

        // Check if the collision is with the EnemyHeadObj
        if (collision.transform == EnemyTail.EnemyHeadObj)
        {
            // Sets the list enemyPositions to be equal to the local list enemyOrbPositions so that when the enemy snake dies the positions are not lost & can be run through the attatchment to the player's snake
            enemyOrbPositions = EnemyTail.enemyPositions;

            // Kills the enemy
            EnemyTail.KillEnemy();
            // Handles orb drops after the enemy has been killed
            EnemyOrbDrops();
        }

        // Check if the collision is with the SnakeHeadObj
        if (collision.transform == SnakeTail.SnakeHeadObj)
        {
            // Kills the player
            SnakeTail.KillPlayer();
            SnakeTail.GameOver();
        }
    }
    private void EnemyOrbDrops()
    {
        // These conditions prevent this from running pre-emptively
        if (!EnemyTail.enemyAlive && !spawningFinished)
        {
            // Using enemyOrbPositions, it stores the number of positions that orbs need to be spawned in as a numerical value
            int orbsToSpawn = enemyOrbPositions.Count;

            // Iterates through the list enemyOrbPositions until an orb has been spawned in every position
            for (int i = 0; i < orbsToSpawn; i++)
            {
                // Picks a random number between 0 & 2
                choice = Random.Range(0, 3);

                // Instantiates the foodObject based on which foodObject has been chosen out of foodChoices at the position stored at the current index in the list enemyOrbPositions
                GameObject newFoodObj = Instantiate(RandomSpawn.foodChoices[choice], enemyOrbPositions[i], Quaternion.identity);
                enemyOrbs.Add(newFoodObj);
            }

            // Ends the condition as spawningFinished in now true
            spawningFinished = true;
        }
    }

}

