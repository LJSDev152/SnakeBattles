using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrow : MonoBehaviour
{
    // Set as public so the script is accessible to other scripts
    public EnemyTail EnemyTail;
    public RandomSpawn RandomSpawn;

    // The code inside OnTriggerEnter2D() has not been put into a separate method as the code must be inside to access the attribute 'collision'

    // Built-in function: Called whenever an object with a RigidBody2D & Collider2D (EnemySnake) collides with an object with a Collider2D set as trigger (Food)
    private void OnTriggerEnter2D(Collider2D collision)
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

            // Destroys the touched object with a trigger collider with slight delay to prevent lag
            Destroy(collision.gameObject, 0.02f);
            // Removes the specific object the enemy snake collides with
            RandomSpawn.foodList.Remove(collision.gameObject);
        }
    }
}
