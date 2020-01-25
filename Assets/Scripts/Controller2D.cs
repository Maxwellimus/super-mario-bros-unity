using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController
{
    public CollisionInfo collisions;

    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform groundCheck;
    const float ceilingRadius = 0.2f;

    public GameObject HitCeilingObject()
    {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ceilingCheck.position, ceilingRadius, collisionMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                return colliders[i].gameObject;
        }

        return null;
    }

    public GameObject HitGroundObject()
    {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, ceilingRadius, collisionMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                return colliders[i].gameObject;
        }

        return null;
    }

    private void HandleVerticalCollisions(ref Vector2 moveAmount)
    {        
        int directionY = (int)Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.yellow);

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
        int directionX = collisions.faceDir;
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

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.yellow);

            if (hit)
            {
                if (System.Math.Abs(hit.distance) < Mathf.Epsilon)
                {
                    continue;
                }

                moveAmount.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;


                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void DrawDebugCollisions()
    {
        if (collisions.above)
        {
            // Draw collision above
            Debug.DrawLine(raycastOrigins.topLeft, raycastOrigins.topRight, Color.green, 3);
        }

        if (collisions.below)
        {
            // Draw collision below
            Debug.DrawLine(raycastOrigins.bottomLeft, raycastOrigins.bottomRight, Color.green, 3);
        }

        if (collisions.left)
        {
            // Draw collision left
            Debug.DrawLine(raycastOrigins.bottomLeft, raycastOrigins.topLeft, Color.green, 3);
        }

        if (collisions.right)
        {
            // Draw collision right
            Debug.DrawLine(raycastOrigins.bottomRight, raycastOrigins.topRight, Color.green, 3);
        }
    }

    public void Move(Vector2 moveDistance)
    {
        if (System.Math.Abs(moveDistance.x) > Mathf.Epsilon)
        {
            collisions.faceDir = (int)Mathf.Sign(moveDistance.x);
        }

        collisions.Reset();
        HandleHorizontalCollisions(ref moveDistance);

        if(System.Math.Abs(moveDistance.y) > Mathf.Epsilon)
        {
            HandleVerticalCollisions(ref moveDistance);
        }

        DrawDebugCollisions();
        transform.Translate(moveDistance);
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
