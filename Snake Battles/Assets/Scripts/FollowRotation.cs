using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    public Transform Player;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Player.rotation;
    }
}
