using UnityEngine;

public class SnakeGrow : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public SnakeTail SnakeTail;
    public RandomSpawn RandomSpawn;
    public KillSnake KillSnake;

    // The code inside OnTriggerEnter2D() has not been put into a separate method as the code must be inside to access the attribute 'collision'

    // Built-in function: Called whenever an object with a RigidBody2D & Collider2D (Snake) collides with an object with a Collider2D set as trigger (Food)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If Snake collides with an object with the tag "Food", run the code below if the player is alive
        if (collision.gameObject.CompareTag("Food") && SnakeTail.snakeAlive)
        {
            // Adding tail to player snake

            // Every time snakeTail.AddTail() is called, the length of the snake increases by 1 so "RedFood(Clone)" would increase the snake's size by 3 when called
            // Individual foods identified using names instead of tags as Food tag is already attached to all food objects
            if (collision.gameObject.name == "RedFood(Clone)")
            {
                SnakeTail.AddTail();
                SnakeTail.AddTail();
                SnakeTail.AddTail();
            }

            if (collision.gameObject.name == "GreenFood(Clone)")
            {
                SnakeTail.AddTail();
                SnakeTail.AddTail();
            }

            if (collision.gameObject.name == "YellowFood(Clone)")
            {
                SnakeTail.AddTail();
            }

            // Destroying the object, updating foodList & updating the size counter

            // Destroys the touched object with a trigger collider with slight delay to prevent lag
            Destroy(collision.gameObject, 0.02f);
            // Removes the specific object the player's snake collides with
            RandomSpawn.foodList.Remove(collision.gameObject);

            // The variable foodAmount is updated by 1 each time it is called so "RedFood(Clone)" will increase foodAmount by 3 for example
            // Individual foods identified using names instead of tags as Food tag is already attached to all food objects
            if (collision.gameObject.name == "RedFood(Clone)")
            {
                RandomSpawn.AddFoodToSizeCounter();
                RandomSpawn.AddFoodToSizeCounter();
                RandomSpawn.AddFoodToSizeCounter();
            }

            if (collision.gameObject.name == "GreenFood(Clone)")
            {
                RandomSpawn.AddFoodToSizeCounter();
                RandomSpawn.AddFoodToSizeCounter();
            }

            if (collision.gameObject.name == "YellowFood(Clone)")
            {
                RandomSpawn.AddFoodToSizeCounter();
            }
        }
    }
}