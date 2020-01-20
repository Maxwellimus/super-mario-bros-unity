using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focusObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(focusObject.transform.position.x, focusObject.transform.position.y, -10);
    }
}
