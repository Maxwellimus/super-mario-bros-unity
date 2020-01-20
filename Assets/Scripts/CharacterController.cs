using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : RaycastController
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;

    private float gravity = 5.0f;
    private CollisionInfo collisions;
    private float horizontal = 0;
    private Vector2 velocity;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        Debug.Log("Starting Player");
        velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Zero out each frame to compute new values
        velocity = Vector2.zero;

        horizontal = Input.GetAxisRaw("Horizontal");
        velocity.x = horizontal * moveSpeed * Time.deltaTime;
        velocity.y = -1 * gravity * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        UpdateRaycastOrigins();
        HandleVerticalCollisions(ref velocity);
        HandleHorizontalCollisions(ref velocity);
        transform.position += new Vector3(velocity.x, velocity.y);
    }

    private void HandleVerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {

                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void HandleHorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                moveAmount.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;


                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision 2d");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D");
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public int faceDir;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
