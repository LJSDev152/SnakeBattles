using System.Collections.Generic;
using UnityEngine;
// Reference for Random
using Random = UnityEngine.Random;

public class RandomEnemySpawn : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public SnakeTail SnakeTail;
    public RandomSpawn RandomSpawn;
    public EnemyTail EnemyTail;

    public List<GameObject> enemyList = new List<GameObject>();

    // SerializeField allows the value held in the variables below to be visible in the inspector
    // All set to private as not used in other scripts
    [SerializeField] private GameObject Enemy;

    private Vector2 pos;

    private float plrDistance;
    private float foodDistance;
    [SerializeField] private float foodNearestDistance = 1000;
    [SerializeField] private float plrNearestDistance = 1000;

    // Built-in function: Called on first frame
    private void Start()
    {
        // Referenced at the start as when an enemy is cloned, the scripts that were previously dragged on in the inspector no longer exist and must be referenced like this instead
        SnakeTail = GameObject.FindGameObjectWithTag("Snake").GetComponent<SnakeTail>();
        RandomSpawn = GameObject.FindGameObjectWithTag("Snake").GetComponent<RandomSpawn>();

        SpawnEnemy();
    }

    // Built-in Function: Called every frame
    private void Update()
    {
        // Referenced in Update() with an null check as the Enemy(Clone) GameObject is spawned in runtime and doesn't exist before the program is ran
        if (EnemyTail == null)
        {
            EnemyTail = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyTail>();
        }

        if (EnemyTail.enemyAlive && Enemy == null)
        {
            Enemy = GameObject.FindGameObjectWithTag("Enemy");
            Debug.Log("Called");
        }

        SpawnEnemy();
    }

    // Everything is stored as a method so they can be easily changed to public & be accessible to other scripts if needed, improving their flexibility, reusability & readability of the code

    private void SpawnEnemy()
    {
        // If the snake is alive & an enemy hasn't been spawned yet, spawn an enemy
        if (SnakeTail.snakeAlive && enemyList.Count < 1)
        {
            RandomEnemySpawning();         
        }
    }

    private void RandomEnemySpawning()
    {
        // Creates 2 points that are 10 units away from the camera plane and will be 5% away from all the edges
        float x = Random.Range(0.05f, 0.95f);
        float y = Random.Range(0.05f, 0.95f);

        // Takes the 2 vectors & translates them to viewport points on the main camera
        pos = new Vector2(x, y);
        pos = Camera.main.ViewportToWorldPoint(pos);

        // Ensures the nearest distance is reset every time SpawnSpacing() is called to prevent a stack overflow from occurring in the case of having to generate another position
        foodNearestDistance = 1000;
        plrNearestDistance = 1000;

        // Handles the spacing of all food when it is spawned in
        SpawnSpacing();
    }

    private void SpawnSpacing()
    {
        // Spawn spacing for player

        for (int i = (SnakeTail.positions).Count - 1; i >= 0; i--)
        {
            // Gets the distance between the position of the current tail object being searched in the list to the random enemy position generated from RandomEnemySpawning()
            plrDistance = Vector2.Distance(SnakeTail.positions[i], pos);

            // If the distance is less than the current nearest distance, the nearest distance is replaced with the current tail distance
            if (plrDistance < plrNearestDistance)
            {
                plrNearestDistance = plrDistance;
            }
        }

        // Spawn spacing for food

        for (int i = (RandomSpawn.foodList).Count - 1; i >= 0; i--)
        {
            // Gets the distance between the position of the current food object being searched in the list to the random enemy position generated from RandomEnemySpawning()
            foodDistance = Vector2.Distance((RandomSpawn.foodList)[i].transform.position, pos);

            // If the distance is less than the current nearest distance, the nearest distance is replaced with the current food distance
            if (foodDistance < foodNearestDistance)
            {
                foodNearestDistance = foodDistance;
            }
        }

        // Confirmation of both conditions being met

        if ((plrNearestDistance > 5 && foodNearestDistance > 2))
        {
            // Generates a random object inside foodChoices at a random viewport position inside the view of the camera
            GameObject newObj = Instantiate(Enemy, pos, Quaternion.identity);
            // The instantiated object is added to the list to prevent trying to delete the prefab, which would cause an error
            enemyList.Add(newObj);
        }

        // If the above conditions aren't met, a new position is generated
        else
        {
            RandomEnemySpawning();
        }
    }
}
