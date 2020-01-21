﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{      
    Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.OnJumpInputUp();
        }

        // Horizontal Move
        player.OnHorizontalMove((int)Input.GetAxisRaw("Horizontal"));
    }
}