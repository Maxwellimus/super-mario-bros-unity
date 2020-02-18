using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Goomba : Enemy
{
    public Controller2D controller2D;
    Vector3 velocity;
    int movingDirection = -1;

    // Acceleration due to gravity
    float gravity = -10;
    // Horizontal speeds
    float moveSpeed = 2f;

    bool alive = true;

    private SpriteAnimationController animationController;


    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponent<SpriteAnimationController>();
        controller2D = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            animationController.PlayAnimation("Walking");
        }
        else
        {
            animationController.PlayAnimation("Dead");
            Destroy(gameObject, 1);
        }
        
    }

    void FixedUpdate()
    {
        CalculateVelocity();
        if (alive)
        {
            controller2D.Move(velocity * Time.fixedDeltaTime);
        }
    }

    void CalculateVelocity()
    {
        if (controller2D.collisions.right)
        {
            movingDirection = -1;
        }
        else if (controller2D.collisions.left)
        {
            movingDirection = 1;
        }

        velocity.x = movingDirection * moveSpeed;

        // Need to multiply gravity by Time.deltaTime since it is an acceleration
        // and we want a velocity. (acceleration = velocity/time)
        velocity.y += gravity * Time.deltaTime;
    }

    public override void Damage()
    {
        alive = false;
    }
}
