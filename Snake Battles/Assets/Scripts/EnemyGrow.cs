using UnityEngine;

public class EnemyGrow : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public EnemyTail EnemyTail;
    public SnakeTail SnakeTail;
    public RandomSpawn RandomSpawn;
    public KillSnake KillSnake;

    // Built-in Function: Called every frame
    private void Update()
    {
        // Referenced in Update() with an null check as the Enemy(Clone) GameObject is spawned in runtime and doesn't exist before the program is ran
        if (EnemyTail == null && SnakeTail.snakeAlive)
        {
            EnemyTail = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyTail>();
        }
    }

    // The code inside OnTriggerEnter2D() has not been put into a separate method as the code must be inside to access the attribute 'collision'

        // Built-in function: Called whenever an object with a RigidBody2D & Collider2D (EnemySnake) collides with an object with a Collider2D set as trigger (Food)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if either the player or enemy has been killed before running the code to prevent possible errors
        if (SnakeTail.snakeAlive && EnemyTail.enemyAlive)
        {
            // Adding tail to enemy snake

            // If Snake collides with an object with the tag "Food", run the code below
            if (collision.gameObject.CompareTag("Food"))
            {
                // Every time EnemyTail.AddTail() is called, the length of the snake increases by 1 so "RedFood(Clone)" would increase the snake's size by 3 when called
                // Individual foods identified using names instead of tags as Food tag is already attached to all food objects
                if (collision.gameObject.name == "RedFood(Clone)")
                {
                    EnemyTail.AddEnemyTail();
                    EnemyTail.AddEnemyTail();
                    EnemyTail.AddEnemyTail();
                }

                if (collision.gameObject.name == "GreenFood(Clone)")
                {
                    EnemyTail.AddEnemyTail();
                    EnemyTail.AddEnemyTail();
                }

                if (collision.gameObject.name == "YellowFood(Clone)")
                {
                    EnemyTail.AddEnemyTail();
                }

                // Destroying the object, updating foodList & updating the size counter

                // Using int variables, we can get the index of a food object in a particular foodList, & store the index in the int
                int opt1 = RandomSpawn.foodList.IndexOf(collision.gameObject);
                int opt2 = KillSnake.enemyOrbs.IndexOf(collision.gameObject);

                // If no index is found, the value of these int variables will be -1
                // Therefore if the value is != -1, an index has been found which allows us to run this code if the index is found & from where it was found
                if (opt1 != -1)
                {
                    RandomSpawn.foodList.RemoveAt(opt1);
                }

                if (opt2 != -1)
                {
                    KillSnake.enemyOrbs.RemoveAt(opt2);
                }

                // The object is destroyed after being removed from its respective list to prevent trying to access missing GameObjects
                Destroy(collision.gameObject);
            }
        }
    }
}
