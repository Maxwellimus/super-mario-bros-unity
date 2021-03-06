﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public Controller2D controller2D;
    Vector3 velocity;
    float velocityXSmoothing;
    int movingDirection;

    bool marioIsBig = false;

    // Vertical speeds
    public float maxJumpHeight = 5;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .15f;
    bool jumping = false;
    bool hitBlockOnJump = false;

    // Acceleration due to gravity
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float enemyJumpVelocity;

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
        enemyJumpVelocity = maxJumpVelocity / 2;

        Debug.Log("gravity: " + gravity);
        Debug.Log("minJumpVelocity: " + minJumpVelocity);
        Debug.Log("maxJumpVelocity: " + maxJumpVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -4)
        {
            SceneManager.LoadScene("Main");
        }

        SetupColliderSize();
    }

    private void FixedUpdate()
    {
        CalculateVelocity();
        MoveMario();
    }

    private void SetupColliderSize()
    {
        float colliderHeight = marioIsBig ? 1.5f:0.75f;
        float ceiling = (int)(colliderHeight + 1);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, (colliderHeight - ceiling) / 2);
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, colliderHeight);
    }

    private void MoveMario()
    {
        controller2D.Move(velocity * Time.fixedDeltaTime);

        // After move check to see if we hit a ceiling or the ground.
        // If so we want to reset velocity.y to 0 as it means we're ending
        // a jump potentially.
        if (controller2D.collisions.above || controller2D.collisions.below)
        {
            velocity.y = 0;
            jumping = false;
            hitBlockOnJump = false;
        }

        if(controller2D.collisions.left || controller2D.collisions.right)
        {
            velocity.x = 0;
        }

        UpdateAnimations();

        if (!hitBlockOnJump)
        {
            GameObject ceilingObject = controller2D.HitCeilingObject();
            GameObject groundObject = controller2D.HitGroundObject();

            if (ceilingObject)
            {
                if (ceilingObject.tag == "BrickBlock")
                {
                    BrickBlock brickBlock = ceilingObject.GetComponent<BrickBlock>();
                    brickBlock.HitBlock();
                    hitBlockOnJump = true;
                }

                if (ceilingObject.tag == "QuestionBlock")
                {
                    QuestionBlock questionBlock = ceilingObject.GetComponent<QuestionBlock>();
                    questionBlock.HitBlock();
                    hitBlockOnJump = true;
                }
            }

            if (groundObject)
            {
                if(groundObject.tag == "Enemy")
                {
                    Enemy enemy = groundObject.GetComponent<Enemy>();
                    enemy.Damage();
                    hitBlockOnJump = true;
                    velocity.y = enemyJumpVelocity;
                    jumping = true;
                }
            }
        }

        GameObject itemObject = controller2D.HitItem();
        if (itemObject)
        {
            if(itemObject.tag == "Mushroom")
            {
                MakeMarioBig();
                Destroy(itemObject);
            }
        }
    }

    private string AnimationForSize(string baseAnimation)
    {
        return baseAnimation + (marioIsBig ? "Big" : "");
    }

    private void UpdateAnimations()
    {
        if (jumping)
        {
            animationController.PlayAnimation(AnimationForSize("Jumping"));
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
            animationController.PlayAnimation(AnimationForSize("Walking"));
        }
        else
        {
            animationController.PlayAnimation(AnimationForSize("Idle"));
        }
    }

    public void OnJumpInputDown()
    {
        Jump(maxJumpVelocity);
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

    private void Jump(float speed)
    {
        if (!controller2D.collisions.below)
        {
            return;
        }

        velocity.y = speed;
        jumping = true;
    }

    private void MakeMarioBig()
    {
        marioIsBig = true;
        UpdateAnimations();

        // Move Mario up half a unit so he doesn't collide with the floor
        controller2D.Move(new Vector2(0, 0.5f));
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
