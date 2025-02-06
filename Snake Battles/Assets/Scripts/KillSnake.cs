using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
// Reference for Random
using Random = UnityEngine.Random;

public class KillSnake : MonoBehaviour
{
    public SnakeTail SnakeTail;
    public EnemyTail EnemyTail;
    public RandomSpawn RandomSpawn;

    [SerializeField] private List<Vector2> enemyOrbPositions = new List<Vector2>();

    private int orbsToSpawn;
    private int choice;
    private bool spawningFinished = false;

    private void Update()
    {
        SpawnPositions();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If both are ran then both snakes are killed
        // If one is run then the one that is run will only kill that snake

        // Check if the collision is with the EnemyHeadObj
        if (collision.transform == EnemyTail.EnemyHeadObj)
        {
            EnemyTail.KillEnemy();
            EnemyOrbDrops();
        }

        // Check if the collision is with the SnakeHeadObj
        if (collision.transform == SnakeTail.SnakeHeadObj)
        {
            SnakeTail.KillPlayer();
        }
    }

    private void EnemyOrbDrops()
    {
        if (!EnemyTail.enemyAlive && ! spawningFinished) {
            Debug.Log("Entered first bracket");
            orbsToSpawn = RandomSpawn.foodAmount;

            for (int i = 0; i < orbsToSpawn; i++)
            {
                Debug.Log("Entered second bracket");
                choice = Random.Range(0, 3);

                for (int j = 0; j < enemyOrbPositions.Count; j++)
                {
                    if (orbsToSpawn - i >= 3)
                    {
                        Debug.Log("Entered a third bracket");
                        GameObject newObj = Instantiate((RandomSpawn.foodChoices)[choice], enemyOrbPositions[j], Quaternion.identity);
                        (RandomSpawn.foodList).Add(newObj);
                    }

                    if (orbsToSpawn - i == 2)
                    {
                        Debug.Log("Entered a third bracket");
                        GameObject newObj = Instantiate((RandomSpawn.foodChoices)[1], enemyOrbPositions[j], Quaternion.identity);
                        (RandomSpawn.foodList).Add(newObj);
                    }

                    if (orbsToSpawn - i == 1)
                    {
                        Debug.Log("Entered a third bracket");
                        GameObject newObj = Instantiate((RandomSpawn.foodChoices)[0], enemyOrbPositions[j], Quaternion.identity);
                        (RandomSpawn.foodList).Add(newObj);
                    }

                    else
                    {
                        spawningFinished = true;
                    }
                }
            }
        }
    }

    private void SpawnPositions()
    {
        if (EnemyTail.enemyAlive)
        {
            for (int i = 0; i < (EnemyTail.enemyPositions).Count; i++)
            {
                enemyOrbPositions.Insert(0, (EnemyTail.enemyPositions)[i]);
                Debug.Log(enemyOrbPositions[i]);
            }
        }
    }
}

