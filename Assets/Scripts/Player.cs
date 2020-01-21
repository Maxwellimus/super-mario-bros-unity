﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public Controller2D controller2D;
    Vector3 velocity;
    float velocityXSmoothing;
    int movingDirection;

    // Vertical speeds
    public float maxJumpHeight = 5;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = .4f;
    float accelerationTimeGrounded = .3f;
    bool jumping = false;

    // Acceleration due to gravity
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;

    // Horizontal speeds
    float moveSpeed = 10f;

    private SpriteAnimationController animationController;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponent<SpriteAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        controller2D = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        Debug.Log("gravity: " + gravity);
        Debug.Log("minJumpVelocity: " + minJumpVelocity);
        Debug.Log("maxJumpVelocity: " + maxJumpVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();
    }

    private void FixedUpdate()
    {
        controller2D.Move(velocity * Time.fixedDeltaTime);

        // After move check to see if we hit a ceiling or the ground.
        // If so we want to reset velocity.y to 0 as it means we're ending
        // a jump potentially.
        if (controller2D.collisions.above || controller2D.collisions.below)
        {
            velocity.y = 0;
            jumping = false;
        }

        if(controller2D.collisions.left || controller2D.collisions.right)
        {
            velocity.x = 0;
        }

        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        if (jumping)
        {
            animationController.PlayAnimation("Jumping");
            return;
        }

        if (!controller2D.collisions.below)
        {
            animationController.StopCurrentAnimation();
            return;
        }

        spriteRenderer.flipX = controller2D.collisions.faceDir == -1;

        if (movingDirection != 0)
        {
            animationController.PlayAnimation("Walking");
        }
        else
        {
            animationController.PlayAnimation("Idle");
        }
    }

    public void OnJumpInputDown()
    {
        if(!controller2D.collisions.below)
        {
            return;
        }

        velocity.y = maxJumpVelocity;
        jumping = true;
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    public void OnHorizontalMove(int direction)
    {
        movingDirection = direction;
    }

    void CalculateVelocity()
    {
        float targetVelocityX = movingDirection * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller2D.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        // Need to multiply gravity by Time.deltaTime since it is an acceleration
        // and we want a velocity. (acceleration = velocity/time)
        velocity.y += gravity * Time.deltaTime;
    }
}
