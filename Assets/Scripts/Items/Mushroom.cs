using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Mushroom : MonoBehaviour
{
    public Controller2D controller2D;
    bool physicsEnabled = false;
    int movingDirection = 1;
    Vector3 velocity;

    float moveSpeed = 3f;
    float gravity = -10;

    void Start()
    {
        controller2D = GetComponent<Controller2D>();
    }

    private void Update()
    {
        CalculateVelocity();
    }

    void FixedUpdate()
    {
        if (physicsEnabled)
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

    public IEnumerator AnimateOutOfBox(Vector3 startPos)
    {
        float mushroomPopTime = 1f;
        Vector3 endPos = new Vector3(startPos.x, startPos.y + 1);

        float i = 0.0f;
        float rate = 1.0f / mushroomPopTime;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }

        physicsEnabled = true;
    }
}
