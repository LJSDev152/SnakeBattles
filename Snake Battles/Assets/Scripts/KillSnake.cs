using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSnake : MonoBehaviour
{
    public SnakeTail SnakeTail;
    public EnemyTail EnemyTail;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with the EnemyHeadObj
        if (collision.transform == EnemyTail.EnemyHeadObj)
        {
            EnemyTail.KillEnemy();
        }

        // Check if the collision is with the SnakeHeadObj
        if (collision.transform == SnakeTail.SnakeHeadObj)
        {
            SnakeTail.KillPlayer();
        }
    }
}

