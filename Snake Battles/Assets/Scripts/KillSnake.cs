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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If both are ran then both snakes are killed
        // If one is run then the one that is run will only kill that snake

        // Check if the collision is with the EnemyHeadObj
        if (collision.transform == EnemyTail.EnemyHeadObj)
        {
            enemyOrbPositions = EnemyTail.enemyPositions;
            orbsToSpawn = enemyOrbPositions.Count;

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
        if (!EnemyTail.enemyAlive && !spawningFinished)
        {
            Debug.Log("Entered first bracket");

            for (int i = 0; i < enemyOrbPositions.Count; i++)
            {
                Debug.Log("Entered second bracket");

                for (int j = 0; j < orbsToSpawn; j++)
                {
                    Debug.Log("Entered third bracket");
                    choice = Random.Range(0, 3);
                    int orbsLeftToSpawn = orbsToSpawn - j;

                    Debug.Log("Choice: " + choice);
                    Debug.Log("orbsToSpawn: " + orbsToSpawn);
                    Debug.Log("orbsLeftToSpawn: " + orbsLeftToSpawn);

                    if (RandomSpawn.foodList.Count < 10)
                    {
                        if (orbsLeftToSpawn >= 3)
                        {
                            Debug.Log("Entered a fourth bracket");
                            GameObject newFoodObj = Instantiate((RandomSpawn.foodChoices)[choice], enemyOrbPositions[i], Quaternion.identity);
                            (RandomSpawn.foodList).Add(newFoodObj);
                            orbsToSpawn -= choice + 1;
                        }

                        if (orbsLeftToSpawn == 2)
                        {
                            Debug.Log("Entered a fourth bracket");
                            GameObject newFoodObj = Instantiate((RandomSpawn.foodChoices)[1], enemyOrbPositions[i], Quaternion.identity);
                            (RandomSpawn.foodList).Add(newFoodObj);
                            orbsToSpawn -= choice + 1;
                        }

                        if (orbsLeftToSpawn == 1)
                        {
                            Debug.Log("Entered a fourth bracket");
                            GameObject newFoodObj = Instantiate((RandomSpawn.foodChoices)[0], enemyOrbPositions[i], Quaternion.identity);
                            (RandomSpawn.foodList).Add(newFoodObj);
                            orbsToSpawn -= choice + 1;
                        }

                        else
                        {
                            spawningFinished = true;
                        }
                    }
                }
            }
        }
    }
}

